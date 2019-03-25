namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("PublicServiceWasRemoved")]
    [EventDescription("De dienstverlening werd verwijderd.")]
    public class PublicServiceWasRemoved
    {
        public string PublicServiceId { get; }
        public string ReasonForRemoval { get; }

        public PublicServiceWasRemoved(
            PublicServiceId publicServiceId,
            ReasonForRemoval reasonForRemoval)
        {
            PublicServiceId = publicServiceId;
            ReasonForRemoval = reasonForRemoval;
        }

        [JsonConstructor]
        private PublicServiceWasRemoved(
            string publicServiceId,
            string reasonForRemoval) :
                this(
                    new PublicServiceId(publicServiceId),
                    new ReasonForRemoval(reasonForRemoval)) { }
    }
}
