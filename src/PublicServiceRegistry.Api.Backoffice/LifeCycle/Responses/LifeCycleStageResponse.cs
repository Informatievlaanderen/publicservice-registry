namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
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
        /// De startdatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 2)]
        public DateTime? Vanaf { get; }

        /// <summary>
        /// De einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 3)]
        public DateTime? Tot { get; }

        public LifeCycleStageResponse(
            string lifeCycleStageType,
            DateTime? from,
            DateTime? to)
        {
            LevensloopfaseType = lifeCycleStageType;
            Vanaf = from;
            Tot = to;
        }
    }

    public class LifeCycleStageResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LifeCycleStageResponse>
            {
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.Active.ToString(), DateTime.Now.Date, DateTime.Now.Date.AddDays(1)),
                new LifeCycleStageResponse(PublicServiceRegistry.LifeCycleStageType.PhasingOut.ToString(), DateTime.Now.Date, DateTime.Now.Date.AddDays(1)),
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
