namespace PublicServiceRegistry.PublicService.Providers.OrganisationProviderSources
{
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class DataVlaanderenOrganisation
    {
        public string Raw { get; private set; }
        public string PrefLabel { get; private set; }

        private DataVlaanderenOrganisation() { }

        public static Optional<DataVlaanderenOrganisation> FromJson(
            string idPropertyName,
            string responseContent)
        {
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(responseContent);

            if (!deserialized.Keys.Any())
                return Optional<DataVlaanderenOrganisation>.Empty;

            var dataVlaanderenIdPropertyString = deserialized[idPropertyName].ToString();
            var dataVlaanderenIdProperty = JsonConvert.DeserializeObject<DataVlaanderenIdProperty>(dataVlaanderenIdPropertyString);

            var prefLabel = dataVlaanderenIdProperty.PrefLabels[0].Value;

            return new Optional<DataVlaanderenOrganisation>(
                new DataVlaanderenOrganisation
                {
                    Raw = responseContent,
                    PrefLabel = prefLabel
                }
            );
        }
    }

    internal class DataVlaanderenIdProperty
    {
        [JsonProperty("http://www.w3.org/2004/02/skos/core#prefLabel")]
        public PrefLabelBlock[] PrefLabels { get; set; }

        public class PrefLabelBlock
        {
            public string Value { get; set; }
        }
    }
}
