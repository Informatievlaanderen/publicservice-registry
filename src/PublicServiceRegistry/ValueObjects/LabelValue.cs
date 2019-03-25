namespace PublicServiceRegistry
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class LabelValue: StringValueObject<LabelValue>
    {
        public LabelValue([JsonProperty("value")] string labelValue) : base(labelValue) { }
    }
}
