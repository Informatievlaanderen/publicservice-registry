namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using NodaTime;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloop", Namespace = "")]
    public class LifeCycleStageResponse
    {
        /// <summary>
        /// Het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseType", Order = 1)]
        public string LevensloopfaseType { get; }

        /// <summary>
        /// Het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseTypeNaam", Order = 2)]
        public string LifeCycleStageTypeNaam { get; }

        /// <summary>
        /// De startdatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 3)]
        public DateTime? Vanaf { get; }

        /// <summary>
        /// De einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 4)]
        public DateTime? Tot { get; }

        public LifeCycleStageResponse(
            string lifeCycleStageType,
            string lifeCycleStageTypeName,
            LocalDate? from,
            LocalDate? to)
        {
            LevensloopfaseType = lifeCycleStageType;
            LifeCycleStageTypeNaam = lifeCycleStageTypeName;
            Vanaf = from?.ToDateTimeUnspecified();
            Tot = to?.ToDateTimeUnspecified();
        }
    }

    public class LifeCycleStageResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LifeCycleStageResponse>
            {
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.Active.ToString(), PublicServiceRegistry.LifeCycleStageType.Active.Translation.Name, LocalDate.FromDateTime(DateTime.Now.Date), LocalDate.FromDateTime(DateTime.Now.Date.AddDays(1))),
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.PhasingOut.ToString(), PublicServiceRegistry.LifeCycleStageType.PhasingOut.Translation.Name, LocalDate.FromDateTime(DateTime.Now.Date), LocalDate.FromDateTime(DateTime.Now.Date.AddDays(1))),
            };
    }

    public class LifeCycleStageNotFoundResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = BasicApiProblem.DefaultTitle,
                Detail = "Onbestaande levensloopfase.",
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
