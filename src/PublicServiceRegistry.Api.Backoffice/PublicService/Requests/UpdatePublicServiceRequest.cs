namespace PublicServiceRegistry.Api.Backoffice.PublicService.Requests
{
    using System.ComponentModel.DataAnnotations;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class UpdatePublicServiceInternalRequest
    {
        public string PublicServiceId { get; set; }
        public UpdatePublicServiceRequest Body { get; set; }

        public UpdatePublicServiceInternalRequest(string publicServiceId, UpdatePublicServiceRequest body)
        {
            PublicServiceId = publicServiceId;
            Body = body;
        }
    }

    public class UpdatePublicServiceRequest
    {
        /// <summary>Naam van de dienstverlening.</summary>
        [Required]
        public string Naam { get; set; }

        /// <summary>Bevoegde autoriteit voor de dienstverlening.</summary>
        [Required]
        public string BevoegdeAutoriteitOvoNummer { get; set; }

        /// <summary>De dienstverlening is van het type subsidie.</summary>
        public bool IsSubsidie { get; set; }
    }

    public class UpdatePublicServiceRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new UpdatePublicServiceRequest
            {
                Naam = "Nieuwe naam",
                BevoegdeAutoriteitOvoNummer = "OVO000000001",
                IsSubsidie = true
            };
    }

    public static class UpdatePublicServiceRequestMapping
    {
        public static UpdatePublicService Map(
            UpdatePublicServiceInternalRequest message) =>
            new UpdatePublicService(
                new PublicServiceId(message.PublicServiceId),
                new PublicServiceName(message.Body.Naam),
                new OvoNumber(message.Body.BevoegdeAutoriteitOvoNummer),
                message.Body.IsSubsidie);
    }
}
