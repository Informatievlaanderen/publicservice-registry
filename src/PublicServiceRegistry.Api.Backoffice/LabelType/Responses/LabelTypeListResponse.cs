namespace PublicServiceRegistry.Api.Backoffice.LabelType.Responses
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Filters;

    public class LabelTypeListResponse
    {
        /// <summary>Id van de alternatieve benamingstype.</summary>
        public string Id { get; }

        public LabelTypeListResponse(string id) => Id = id;
    }

    public class LabelTypeListResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LabelTypeListResponse>
            {
                new LabelTypeListResponse("Ipdc"),
                new LabelTypeListResponse("Subsidieregister")
            };
    }
}
