namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class PublicServiceId : StringValueObject<PublicServiceId>
    {
        public PublicServiceId([JsonProperty("value")] string publicServiceId) : base(publicServiceId)
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new ArgumentException(nameof(publicServiceId));

            if (!Value.StartsWith("DVR", StringComparison.Ordinal))
                throw new ArgumentException(nameof(publicServiceId));

            var numberAsString = Value.Substring(3);
            if (numberAsString.Length != 9)
                throw new ArgumentException(nameof(publicServiceId));

            var parsed = !int.TryParse(numberAsString, out var result);
            if (parsed)
                throw new ArgumentException(nameof(publicServiceId));

            if (result <= 0)
                throw new ArgumentException(nameof(publicServiceId));
        }

        public static PublicServiceId FromNumber(int number)
            => new PublicServiceId($"DVR{number:D9}");
    }
}
