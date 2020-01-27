namespace PublicServiceRegistry.Api.Backoffice.PublicService.Requests
{
    using System.ComponentModel.DataAnnotations;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class RegisterPublicServiceRequest
    {
        /// <summary>Naam voor de dienstverlening.</summary>
        [Required]
        public string Naam { get; set; }
    }

    public class RegisterPublicServiceRequestExample : IExamplesProvider<RegisterPublicServiceRequest>
    {
        public RegisterPublicServiceRequest GetExamples() =>
            new RegisterPublicServiceRequest
            {
                Naam = "Schooltoelage voor het basisonderwijs en het secundair onderwijs"
            };
    }

    public static class RegisterPublicServiceRequestMapping
    {
        public static RegisterPublicService Map(
            RegisterPublicServiceRequest message,
            PublicServiceId publicServiceId) =>
            new RegisterPublicService(
                publicServiceId,
                new PublicServiceName(message.Naam),
                PrivateZoneId.Unregistered);
    }
}
