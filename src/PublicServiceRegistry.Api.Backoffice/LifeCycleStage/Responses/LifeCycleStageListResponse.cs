namespace PublicServiceRegistry.Api.Backoffice.LifeCycleStage.Responses
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Filters;

    public class LifeCycleStageListResponse
    {
        /// <summary>Id van de levensfase.</summary>
        public string Id { get; }

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
