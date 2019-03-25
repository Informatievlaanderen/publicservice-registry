namespace PublicServiceRegistry.Tests.OrganisationProvider
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublicService;
    using PublicService.Providers;
    using PublicService.Providers.OrganisationProviderSources;
    using Xunit;

    public class WhenRequestingAnOrganisationWhichIsFoundInDataVlaanderen
    {
        private OrganisationProvider _sut;
        private readonly Organisation _requestedOrganisation;
        private OrganisationName _datavlaanderenOrganisationName;
        private OvoNumber _ovoNumber;
        private string _provenanceUri;
        private Mock<IWegwijsProvider> _wegwijsProviderMock;

        public WhenRequestingAnOrganisationWhichIsFoundInDataVlaanderen()
        {
            SetupContext();

            _requestedOrganisation = _sut.GetOrganisationAsync(_ovoNumber).GetAwaiter().GetResult();
        }

        private void SetupContext()
        {
            _ovoNumber = new OvoNumber("OVO000015");
            _datavlaanderenOrganisationName = new OrganisationName("Departement van verloren gebouwen");
            _provenanceUri = "uri//11";

            var dataVlaanderenProviderMock = new Mock<IDataVlaanderenProvider>();
            _wegwijsProviderMock = new Mock<IWegwijsProvider>();

            dataVlaanderenProviderMock
                .Setup(provider => provider.GetOrganisationAsync(_ovoNumber))
                .ReturnsAsync(() => new Optional<Organisation>(
                    new Organisation
                    {
                        Name = _datavlaanderenOrganisationName,
                        OvoNumber = _ovoNumber,
                        Provenance = OrganisationProvenance.FromDataVlaanderen(_provenanceUri)
                    }));

            _sut = new OrganisationProvider(
                Mock.Of<ILogger<OrganisationProvider>>(),
                dataVlaanderenProviderMock.Object,
                _wegwijsProviderMock.Object);
        }

        [Fact]
        public void ThenTheNameShouldBeTheDataVlaanderenProvidedName()
        {
            _requestedOrganisation.Name.Should().Be(_datavlaanderenOrganisationName);
        }

        [Fact]
        public void ThenTheOvoNumberShouldBeGivenOvoNumber()
        {
            _requestedOrganisation.OvoNumber.Should().Be(_ovoNumber);
        }

        [Fact]
        public void ThenTheProvenanceSourceShouldBeDataVlaanderen()
        {
            _requestedOrganisation.Provenance.Source.Should().Be(OrganisationSource.DataVlaanderen);
        }

        [Fact]
        public void ThenTheProvenanceUriShouldBeTheProvidedUri()
        {
            _requestedOrganisation.Provenance.Uri.Should().Be(_provenanceUri);
        }

        [Fact]
        public void ThenTheWegWijsSourceWasNotCalled()
        {
            _wegwijsProviderMock.Verify(provider => provider.GetOrganisationAsync(It.IsAny<OvoNumber>()), Times.Never);
        }
    }
}
