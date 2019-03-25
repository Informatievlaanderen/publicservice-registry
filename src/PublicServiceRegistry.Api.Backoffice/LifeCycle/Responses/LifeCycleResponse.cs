namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System.Collections.Generic;
    using Labels.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class LifeCycleResponse
    {
        /// <summary>Id van het alternatieve benamingstype.</summary>
        public string Id { get; }

        /// <summary>Alternatieve benaming.</summary>
        public string Naam { get; }

        public LifeCycleResponse(
            string id,
            string name)
        {
            Id = id;
            Naam = name;
        }
    }

    public class LifeCycleResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new List<LabelListResponse>
            {
                new LabelListResponse("Ipdc", "Werkplek duaal leren"),
                new LabelListResponse("Subsidieregister", "Duaal leren: werkplek")
            };
    }
}
