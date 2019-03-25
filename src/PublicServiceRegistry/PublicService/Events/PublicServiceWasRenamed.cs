namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("PublicServiceWasRenamed")]
    [EventDescription("De dienstverlening werd hernoemd.")]
    public class PublicServiceWasRenamed
    {
        public string PublicServiceId { get; }
        public string NewName { get; }

        public PublicServiceWasRenamed(
            PublicServiceId publicServiceId,
            PublicServiceName newName)
        {
            PublicServiceId = publicServiceId;
            NewName = newName;
        }

        [JsonConstructor]
        private PublicServiceWasRenamed(
            string publicServiceId,
            string newName) :
                this(
                    new PublicServiceId(publicServiceId),
                    new PublicServiceName(newName)) { }
    }
}
