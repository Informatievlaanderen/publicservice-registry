namespace PublicServiceRegistry.Api.Backoffice.LifeCycleStageType.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloopfase", Namespace = "")]
    public class LifeCycleStageTypeListResponse
    {
        /// <summary>Id van de levensfase.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        public LifeCycleStageTypeListResponse(string id) => Id = id;
    }

    public class LifeCycleStageTypeListResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LifeCycleStageTypeListResponse>
            {
                new LifeCycleStageTypeListResponse("Ipdc"),
                new LifeCycleStageTypeListResponse("Subsidieregister")
            };
    }
}
