namespace PublicServiceRegistry
{
    using System;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class LegislativeDocumentId : StringValueObject<LegislativeDocumentId>
    {
        public LegislativeDocumentId([JsonProperty("value")] string legislativeDocumentId) : base(legislativeDocumentId)
        {
            if (legislativeDocumentId == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(legislativeDocumentId))
                throw new ArgumentException();

            if (!legislativeDocumentId.All(char.IsNumber))
                throw new ArgumentException();
        }
    }
}
