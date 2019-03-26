namespace PublicServiceRegistry.Api.Backoffice.Labels.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Label", Namespace = "")]
    public class LabelListResponse
    {
        /// <summary>Id van het alternatieve benamingstype.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>Alternatieve benaming.</summary>
        [DataMember(Name = "Naam", Order = 2)]
        public string Naam { get; set; }

        public LabelListResponse(
            string id,
            string name)
        {
            Id = id;
            Naam = name;
        }
    }

    public class LabelListResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LabelListResponse>
            {
                new LabelListResponse("Ipdc", "Werkplek duaal leren"),
                new LabelListResponse("Subsidieregister", "Duaal leren: werkplek")
            };
    }
}
