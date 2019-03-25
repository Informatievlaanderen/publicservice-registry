namespace PublicServiceRegistry.Tests.OrganisationProvider
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublicService;
    using PublicService.Providers;
    using PublicService.Providers.OrganisationProviderSources;
    using Xunit;

    public class WhenRequestingAnOrganisationFoundInTheFallBackWegWijsSource
    {
        private OrganisationProvider _sut;
        private readonly Organisation _requestedOrganisation;
        private OrganisationName _wegwijsOrganisationName;
        private OvoNumber _ovoNumber;
        private string _provenanceUri;
        private Mock<IDataVlaanderenProvider> _dataVlaanderenProviderMock;
        private Mock<IWegwijsProvider> _wegwijsProviderMock;

        public WhenRequestingAnOrganisationFoundInTheFallBackWegWijsSource()
        {
            SetupContext();

            _requestedOrganisation = _sut.GetOrganisationAsync(_ovoNumber).GetAwaiter().GetResult();
        }

        private void SetupContext()
        {
            _ovoNumber = new OvoNumber("OVO000015");
            _wegwijsOrganisationName = new OrganisationName("Departement van verloren straten");
            _provenanceUri = "uri//12";

            _dataVlaanderenProviderMock = new Mock<IDataVlaanderenProvider>();
            _wegwijsProviderMock = new Mock<IWegwijsProvider>();

            _dataVlaanderenProviderMock
                .Setup(provider => provider.GetOrganisationAsync(_ovoNumber))
                .ThrowsAsync(new Exception());

            _dataVlaanderenProviderMock
                .Setup(provider => provider.GetOrganisationFailedMessage())
                .Returns("error message");

            _wegwijsProviderMock
                .Setup(provider => provider.GetOrganisationAsync(_ovoNumber))
                .ReturnsAsync(() => new Optional<Organisation>(
                        new Organisation
                        {
                            Name = _wegwijsOrganisationName,
                            OvoNumber = _ovoNumber,
                            Provenance = OrganisationProvenance.FromWegwijsSearch(_provenanceUri)
                        }));

            _sut = new OrganisationProvider(
                Mock.Of<ILogger<OrganisationProvider>>(),
                _dataVlaanderenProviderMock.Object,
                _wegwijsProviderMock.Object);
        }

        [Fact]
        public void ThenTheOvoNumberShouldBeGivenOvoNumber()
        {
            _requestedOrganisation.OvoNumber.Should().Be(_ovoNumber);
        }

        [Fact]
        public void ThenTheProvenanceSourceShouldBeWegWijsSearch()
        {
            _requestedOrganisation.Provenance.Source.Should().Be(OrganisationSource.WegwijsSearch);
        }

        [Fact]
        public void ThenTheProvenanceUriShouldBeTheProvidedUri()
        {
            _requestedOrganisation.Provenance.Uri.Should().Be(_provenanceUri);
        }

        [Fact]
        public void ThenTheDataVlaanderenOrganisationNotFoundMessageWasCalled()
        {
            _dataVlaanderenProviderMock.Verify(provider => provider.GetOrganisationFailedMessage(), Times.Once);
        }
    }
}
