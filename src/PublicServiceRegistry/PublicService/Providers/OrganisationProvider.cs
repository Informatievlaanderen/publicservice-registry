namespace PublicServiceRegistry.PublicService.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Extensions.Logging;
    using OrganisationProviderSources;

    public interface IOrganisationProvider
    {
        Task<Organisation> GetOrganisationAsync(OvoNumber ovoNumber);
    }

    public class OrganisationProvider : IOrganisationProvider
    {
        private readonly ILogger<OrganisationProvider> _logger;
        private readonly IDataVlaanderenProvider _dataVlaanderenProvider;
        private readonly IWegwijsProvider _wegwijsProvider;

        public OrganisationProvider(
            ILogger<OrganisationProvider> logger,
            IDataVlaanderenProvider dataVlaanderenProvider,
            IWegwijsProvider wegwijsProvider)
        {
            _logger = logger;
            _dataVlaanderenProvider = dataVlaanderenProvider;
            _wegwijsProvider = wegwijsProvider;
        }

        public async Task<Organisation> GetOrganisationAsync(OvoNumber ovoNumber)
        {
            var orderedProviderSources = new IOrganisationProviderSource[]
            {
                _dataVlaanderenProvider,
                _wegwijsProvider
            };

            var attemptResults = new List<string>();
            foreach (var provider in orderedProviderSources)
            {
                try
                {
                    var organisation = await provider.GetOrganisationAsync(ovoNumber);
                    if (organisation.HasValue)
                        return organisation.Value;

                    attemptResults.Add(provider.GetOrganisationNotFoundMessage());
                }
                catch (Exception exception)
                {
                    var message = provider.GetOrganisationFailedMessage();
                    _logger.LogError(exception, message);
                    attemptResults.Add(message);
                }
            }

            throw new GetOrganisationFailed(attemptResults);
        }
    }
}
