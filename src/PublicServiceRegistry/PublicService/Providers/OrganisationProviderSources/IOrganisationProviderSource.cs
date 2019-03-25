namespace PublicServiceRegistry.PublicService.Providers.OrganisationProviderSources
{
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IOrganisationProviderSource
    {
        Task<Optional<Organisation>> GetOrganisationAsync(OvoNumber ovoNumber);

        string GetOrganisationNotFoundMessage();

        string GetOrganisationFailedMessage();
    }
}
