namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("LegislativeDocumentIdWasSet")]
    [EventDescription("Er werd een id van een wetgevend document aan de dienstverlening toegewezen.")]
    public class LegislativeDocumentIdWasSet
    {
        public string PublicServiceId { get; }
        public string LegislativeDocumentId { get; }

        public LegislativeDocumentIdWasSet(
            PublicServiceId publicServiceId,
            LegislativeDocumentId legislativeDocumentId)
        {
            PublicServiceId = publicServiceId;
            LegislativeDocumentId = legislativeDocumentId;
        }

        [JsonConstructor]
        private LegislativeDocumentIdWasSet(
            string publicServiceId,
            string legislativeDocumentId) :
            this(
                new PublicServiceId(publicServiceId),
                new LegislativeDocumentId(legislativeDocumentId)) { }
    }
}
