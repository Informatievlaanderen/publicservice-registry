namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("PublicServiceWasRegistered")]
    [EventDescription("De dienstverlening werd geregistreerd.")]
    public class PublicServiceWasRegistered
    {
        public string PublicServiceId { get; }
        public string Name { get; }
        public string PrivateZoneId { get; }

        public PublicServiceWasRegistered(
            PublicServiceId publicServiceId,
            PublicServiceName name,
            PrivateZoneId privateZoneId)
        {
            PublicServiceId = publicServiceId;
            Name = name;
            PrivateZoneId = privateZoneId;
        }

        [JsonConstructor]
        private PublicServiceWasRegistered(
            string publicServiceId,
            string name,
            string privateZoneId) :
            this(
                new PublicServiceId(publicServiceId),
                new PublicServiceName(name),
                new PrivateZoneId(privateZoneId)) { }
    }
}
