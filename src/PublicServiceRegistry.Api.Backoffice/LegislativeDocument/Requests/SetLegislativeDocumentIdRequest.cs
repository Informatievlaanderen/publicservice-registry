namespace PublicServiceRegistry.Api.Backoffice.LegislativeDocument.Requests
{
    using System.ComponentModel.DataAnnotations;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class SetLegislativeDocumentIdRequest
    {
        /// <summary>Id van het wetgevend document voor de dienstverlening.</summary>
        [Required]
        public string LegislativeDocumentId { get; set; }
    }

    public class SetLegislativeDocumentIdRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new SetLegislativeDocumentIdRequest
            {
                LegislativeDocumentId = "1030039"
            };
    }

    public static class SetLegislativeDocumentIdRequestMapping
    {
        public static SetLegislativeDocumentId Map(
            string publicServiceId,
            SetLegislativeDocumentIdRequest message) =>
            new SetLegislativeDocumentId(
                new PublicServiceId(publicServiceId),
                new LegislativeDocumentId(message.LegislativeDocumentId));
    }
}
