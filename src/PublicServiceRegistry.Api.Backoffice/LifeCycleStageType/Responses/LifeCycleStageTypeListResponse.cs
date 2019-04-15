namespace PublicServiceRegistry.Api.Backoffice.LifeCycleStageType.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;
    using LifeCycleStageType = PublicServiceRegistry.LifeCycleStageType;

    [DataContract(Name = "LevensloopfaseType", Namespace = "")]
    public class LifeCycleStageTypeListResponse
    {
        /// <summary>Id van het type levensloopfase.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>Naam van het type levensloopfase.</summary>
        [DataMember(Name = "Naam", Order = 2)]
        public string Naam { get; set; }

        public LifeCycleStageTypeListResponse(LifeCycleStageType lifeCycleStageType)
        {
            Id = lifeCycleStageType.ToString();
            Naam = lifeCycleStageType.Translation.Name;
        }
    }

    public class LifeCycleStageTypeListResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LifeCycleStageTypeListResponse>
            {
                new LifeCycleStageTypeListResponse(LifeCycleStageType.Active),
                new LifeCycleStageTypeListResponse(LifeCycleStageType.PhasingOut)
            };
    }
}
