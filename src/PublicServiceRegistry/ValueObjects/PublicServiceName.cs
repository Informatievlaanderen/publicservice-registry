namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class PublicServiceName : StringValueObject<PublicServiceName>
    {
        public PublicServiceName([JsonProperty("value")] string publicServiceName) : base(publicServiceName)
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new ArgumentException(nameof(publicServiceName));
        }
    }
}
