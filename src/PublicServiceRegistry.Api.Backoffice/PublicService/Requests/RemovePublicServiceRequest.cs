namespace PublicServiceRegistry.Api.Backoffice.PublicService.Requests
{
    using System.ComponentModel.DataAnnotations;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class RemovePublicServiceRequest
    {
        /// <summary>Reden voor de verwijdering van de dienstverlening.</summary>
        [Required]
        public string Reden { get; set; }
    }

    public class RemovePublicServiceRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new RemovePublicServiceRequest
            {
                Reden = "De reden waarom deze dienstverlening mag verwijderd worden.",
            };
    }

    public static class RemovePublicServiceRequestMapping
    {
        public static RemovePublicService Map(
            string publicServiceId,
            RemovePublicServiceRequest message) =>
            new RemovePublicService(
                new PublicServiceId(publicServiceId),
                new ReasonForRemoval(message.Reden));
    }
}
