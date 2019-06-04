namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("IpdcCodeWasSet")]
    [EventDescription("Er werd een ipdc code aan de dienstverlening toegewezen.")]
    public class IpdcCodeWasSet
    {
        public string PublicServiceId { get; }
        public string IpdcCode { get; }

        public IpdcCodeWasSet(
            PublicServiceId publicServiceId,
            IpdcCode ipdcCode)
        {
            PublicServiceId = publicServiceId;
            IpdcCode = ipdcCode;
        }

        [JsonConstructor]
        private IpdcCodeWasSet(
            string publicServiceId,
            string ipdcCode) :
            this(
                new PublicServiceId(publicServiceId),
                new IpdcCode(ipdcCode)) { }
    }
}
