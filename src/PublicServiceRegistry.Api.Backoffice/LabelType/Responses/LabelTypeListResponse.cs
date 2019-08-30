namespace PublicServiceRegistry.Api.Backoffice.LabelType.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Labeltype", Namespace = "")]
    public class LabelTypeListResponse
    {
        /// <summary>Id van de alternatieve benamingstype.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; private set; }

        public LabelTypeListResponse(string id) => Id = id;
    }

    public class LabelTypeListResponseExamples : IExamplesProvider
    {
        public object GetExamples()
            => new List<LabelTypeListResponse>
            {
                new LabelTypeListResponse("Ipdc"),
                new LabelTypeListResponse("Subsidieregister")
            };
    }
}
