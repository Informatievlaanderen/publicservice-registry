namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.ExceptionHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using PublicServiceRegistry.PublicService.Exceptions;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    public class GetOrganisationFailedHandler : IExceptionHandler
    {
        public Type HandledExceptionType => typeof(GetOrganisationFailed);
        public bool Handles(Exception exception) => null != Cast(exception);

        private static GetOrganisationFailed Cast(Exception exception) => exception as GetOrganisationFailed;

        public Task<ProblemDetails> GetApiProblemFor(Exception exception)
        {
            var problem = GetApiProblemFor(Cast(exception));
            return  Task.FromResult<ProblemDetails>(problem);
        }

        public OrganisationNotFoundApiProblem GetApiProblemFor(GetOrganisationFailed exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new OrganisationNotFoundApiProblem
            {
                ProblemInstanceUri = ProblemDetails.GetProblemNumber(),
                HttpStatus = StatusCodes.Status400BadRequest,
                ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception),
                Title = ProblemDetails.DefaultTitle,
                Detail = exception.Message,
                Attempts = exception.AttemptResults.ToList()
            };
        }
    }

    public class OrganisationNotFoundApiProblem : ProblemDetails
    {
        public IEnumerable<string> Attempts { get; set; }
    }
}
