namespace PublicServiceRegistry.PublicService.Providers.OrganisationProviderSources
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IDataVlaanderenProvider : IOrganisationProviderSource {}

    public class DataVlaanderenProvider : IDataVlaanderenProvider
    {
        private const string DataVlaanderenBaseUri = "https://data.vlaanderen.be/id/organisatie/";

        private readonly HttpClient _dataVlaanderenClient;

        public DataVlaanderenProvider()
        {
            // TODO: We moeten met Geert praten hierover, blijkbaar zijn er issues met HttpClient
            // Hij heeft RestSharp client liggen die dit oplost (moeten het ook in andere projecten aanpassen)

            // TODO: Use HttpFactory
            _dataVlaanderenClient = new HttpClient
            {
                BaseAddress = new Uri(DataVlaanderenBaseUri),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }

        public async Task<Optional<Organisation>> GetOrganisationAsync(OvoNumber ovoNumber)
        {
            // TODO: Use HttpFactory
            var httpResponseMessage = await _dataVlaanderenClient.GetAsync(ovoNumber);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            var dataVlaanderenOrganisation = DataVlaanderenOrganisation.FromJson(
                DataVlaanderenBaseUri + ovoNumber,
                responseContent);

            if (dataVlaanderenOrganisation.HasValue)
                return new Optional<Organisation>(
                    new Organisation
                    {
                        Name = new OrganisationName(dataVlaanderenOrganisation.Value.PrefLabel),
                        OvoNumber = ovoNumber,
                        Provenance = OrganisationProvenance.FromDataVlaanderen(httpResponseMessage.RequestMessage.RequestUri.ToString())
                    });

            return Optional<Organisation>.Empty;
        }

        public string GetOrganisationNotFoundMessage() => "De organisatie werd niet gevonden bij data.vlaanderen.be";

        public string GetOrganisationFailedMessage() => "Er is een fout opgetreden bij het ophalen van de organisatie uit data.vlaanderen.be";
    }
}
