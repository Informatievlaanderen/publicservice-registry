namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("LabelWasAssigned")]
    [EventDescription("Er werd een alternatieve benaming van een bepaald type toegekend aan de dienstverlening.")]
    public class LabelWasAssigned
    {
        public string PublicServiceId { get; }
        public string LabelType { get; }
        public string LabelValue { get; }

        public LabelWasAssigned(
            PublicServiceId publicServiceId,
            LabelType labelType,
            LabelValue labelValue)
        {
            PublicServiceId = publicServiceId;
            LabelType = labelType;
            LabelValue = labelValue;
        }

        [JsonConstructor]
        private LabelWasAssigned(
            string publicServiceId,
            string labelType,
            string labelValue) :
            this(
                new PublicServiceId(publicServiceId),
                PublicServiceRegistry.LabelType.Parse(labelType),
                new LabelValue(labelValue)) { }
    }
}
