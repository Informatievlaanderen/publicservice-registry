namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.ExceptionHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using PublicServiceRegistry.PublicService.Exceptions;

    public class GetOrganisationFailedHandler : IExceptionHandler
    {
        public Type HandledExceptionType => typeof(GetOrganisationFailed);
        public bool Handles(Exception exception) => null != Cast(exception);

        private static GetOrganisationFailed Cast(Exception exception) => exception as GetOrganisationFailed;

        public Task<BasicApiProblem> GetApiProblemFor(Exception exception)
        {
            var problem = GetApiProblemFor(Cast(exception));
            return  Task.FromResult<BasicApiProblem>(problem);
        }

        public OrganisationNotFoundApiProblem GetApiProblemFor(GetOrganisationFailed exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new OrganisationNotFoundApiProblem
            {
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber(),
                HttpStatus = StatusCodes.Status400BadRequest,
                ProblemTypeUri = BasicApiProblem.GetTypeUriFor(exception),
                Title = BasicApiProblem.DefaultTitle,
                Detail = exception.Message,
                Attempts = exception.AttemptResults.ToList()
            };
        }
    }

    public class OrganisationNotFoundApiProblem : BasicApiProblem
    {
        public IEnumerable<string> Attempts { get; set; }
    }
}
