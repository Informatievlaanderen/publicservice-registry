namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class ReasonForRemoval : StringValueObject<ReasonForRemoval>
    {
        public ReasonForRemoval([JsonProperty("value")] string reasonForRemoval) : base(reasonForRemoval)
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new ArgumentException(nameof(reasonForRemoval));
        }
    }
}
