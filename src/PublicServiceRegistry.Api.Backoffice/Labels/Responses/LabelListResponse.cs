namespace PublicServiceRegistry.Api.Backoffice.Labels.Responses
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Filters;

    public class LabelListResponse
    {
        /// <summary>Id van het alternatieve benamingstype.</summary>
        public string Id { get; }

        /// <summary>Alternatieve benaming.</summary>
        public string Naam { get; }

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
