namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;
    using NodaTime;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [DataContract(Name = "Levensloop", Namespace = "")]
    public class LifeCycleStageResponse
    {
        /// <summary>
        /// Het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseType", Order = 1)]
        public string LevensloopfaseType { get; private set; }

        /// <summary>
        /// Het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseTypeNaam", Order = 2)]
        public string LifeCycleStageTypeNaam { get; private set; }

        /// <summary>
        /// De startdatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 3)]
        public string Vanaf { get; private set; }

        /// <summary>
        /// De einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 4)]
        public string Tot { get; private set; }

        public LifeCycleStageResponse(
            string lifeCycleStageType,
            string lifeCycleStageTypeName,
            LocalDate? from,
            LocalDate? to)
        {
            LevensloopfaseType = lifeCycleStageType;
            LifeCycleStageTypeNaam = lifeCycleStageTypeName;
            Vanaf = from?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Tot = to?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }

    public class LifeCycleStageResponseExamples : IExamplesProvider<List<LifeCycleStageResponse>>
    {
        public List<LifeCycleStageResponse> GetExamples()
            => new List<LifeCycleStageResponse>
            {
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.Active.ToString(), PublicServiceRegistry.LifeCycleStageType.Active.Translation.Name, LocalDate.FromDateTime(DateTime.Now.Date), LocalDate.FromDateTime(DateTime.Now.Date.AddDays(1))),
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.PhasingOut.ToString(), PublicServiceRegistry.LifeCycleStageType.PhasingOut.Translation.Name, LocalDate.FromDateTime(DateTime.Now.Date), LocalDate.FromDateTime(DateTime.Now.Date.AddDays(1))),
            };
    }

    public class LifeCycleStageNotFoundResponseExamples : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
            => new ProblemDetails
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = ProblemDetails.DefaultTitle,
                Detail = "Onbestaande levensloopfase.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
