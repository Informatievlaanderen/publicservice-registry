namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Labels.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloop", Namespace = "")]
    public class LifeCycleResponse
    {
        /// <summary>Id van het alternatieve benamingstype.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; private set; }

        /// <summary>Alternatieve benaming.</summary>
        [DataMember(Name = "Naam", Order = 2)]
        public string Naam { get; private set; }

        public LifeCycleResponse(
            string id,
            string name)
        {
            Id = id;
            Naam = name;
        }
    }

    public class LifeCycleResponseExamples : IExamplesProvider<List<LabelListResponse>>
    {
        public List<LabelListResponse> GetExamples()
            => new List<LabelListResponse>
            {
                new LabelListResponse("Ipdc", "Werkplek duaal leren"),
                new LabelListResponse("Subsidieregister", "Duaal leren: werkplek")
            };
    }
}
