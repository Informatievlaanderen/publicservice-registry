namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class PrivateZoneId : StringValueObject<PrivateZoneId>
    {
        private const string UnregisteredPrivateZoneId = "UNREGISTERED";

        public static PrivateZoneId Unregistered => new PrivateZoneId(UnregisteredPrivateZoneId);

        public PrivateZoneId([JsonProperty("value")] string privateZoneId) : base(privateZoneId)
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new ArgumentException(nameof(privateZoneId));
        }
    }
}
