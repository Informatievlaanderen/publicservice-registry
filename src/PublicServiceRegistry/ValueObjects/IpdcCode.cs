namespace PublicServiceRegistry
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class IpdcCode: StringValueObject<IpdcCode>
    {
        public IpdcCode([JsonProperty("value")] string ipdcCode) : base(ipdcCode) { }
    }
}