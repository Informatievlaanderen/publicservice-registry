namespace PublicServiceRegistry.Api.Backoffice.LifeCycleStage.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloopfase", Namespace = "")]
    public class LifeCycleStageListResponse
    {
        /// <summary>Id van de levensfase.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        public LifeCycleStageListResponse(string id) => Id = id;
    }

    public class LifeCycleStageListResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LifeCycleStageListResponse>
            {
                new LifeCycleStageListResponse("Ipdc"),
                new LifeCycleStageListResponse("Subsidieregister")
            };
    }
}
