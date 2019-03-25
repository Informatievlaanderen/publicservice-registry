namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("OrafinExportPropertyWasSet")]
    [EventDescription("De organisatie werd al dan niet gemarkeerd voor orafin export.")]
    public class OrafinExportPropertyWasSet
    {
        public string PublicServiceId { get; }
        public bool ExportToOrafin { get; }

        public OrafinExportPropertyWasSet(
            PublicServiceId publicServiceId,
            bool exportToOrafin)
        {
            PublicServiceId = publicServiceId;
            ExportToOrafin = exportToOrafin;
        }

        [JsonConstructor]
        private OrafinExportPropertyWasSet(
            string publicServiceId,
            bool exportToOrafin) :
            this(
                new PublicServiceId(publicServiceId),
                exportToOrafin) { }
    }
}
