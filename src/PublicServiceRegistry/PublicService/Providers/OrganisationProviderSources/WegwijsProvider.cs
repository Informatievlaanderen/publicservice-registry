namespace PublicServiceRegistry.PublicService.Providers.OrganisationProviderSources
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public interface IWegwijsProvider : IOrganisationProviderSource { }

    public class WegwijsProvider : IWegwijsProvider
    {
        private const string WegwijsBaseUri = "https://api.wegwijs.vlaanderen.be/v1/search/";

        private readonly HttpClient _wegwijsVlaanderenClient;

        public WegwijsProvider()
        {
            // TODO: Use HttpFactory
            _wegwijsVlaanderenClient = new HttpClient
            {
                BaseAddress = new Uri(WegwijsBaseUri),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }

        public async Task<Optional<Organisation>> GetOrganisationAsync(OvoNumber ovoNumber)
        {
            // TODO: Use HttpFactory
            var responseMessage = await _wegwijsVlaanderenClient.GetAsync("organisations?q=ovoNumber:" + ovoNumber);

            var responseText = await responseMessage.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<WegwijsOrganisation[]>(responseText);
            if (searchResult.Length != 1)
                return Optional<Organisation>.Empty;

            var organisation = searchResult.Single();
            return new Optional<Organisation>(
                new Organisation
                {
                    Name = new OrganisationName(organisation.Name),
                    OvoNumber = ovoNumber,
                    Provenance = OrganisationProvenance.FromWegwijsSearch(responseMessage.RequestMessage.RequestUri.ToString())
                });
        }

        public string GetOrganisationNotFoundMessage() => "De organisatie werd niet gevonden bij wegwijs.vlaanderen.be";

        public string GetOrganisationFailedMessage() => "Er is een fout opgetreden bij het ophalen van de organisatie uit wegwijs.vlaanderen.be";
    }
}
