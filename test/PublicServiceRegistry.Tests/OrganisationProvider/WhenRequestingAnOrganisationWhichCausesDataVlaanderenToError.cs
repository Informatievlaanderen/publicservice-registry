namespace PublicServiceRegistry.Tests.OrganisationProvider
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublicService;
    using PublicService.Providers;
    using PublicService.Providers.OrganisationProviderSources;
    using Xunit;

    public class WhenRequestingAnOrganisationWhichCausesDataVlaanderenToError
    {
        private OrganisationProvider _sut;
        private OvoNumber _ovoNumber;
        private Mock<IDataVlaanderenProvider> _dataVlaanderenProviderMock;
        private Mock<IWegwijsProvider> _wegwijsProviderMock;

        public WhenRequestingAnOrganisationWhichCausesDataVlaanderenToError()
        {
            SetupContext();

            _sut.GetOrganisationAsync(_ovoNumber).GetAwaiter().GetResult();
        }

        private void SetupContext()
        {
            _ovoNumber = new OvoNumber("OVO000015");

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
                        Name = new OrganisationName("Departement van verloren straten"),
                        OvoNumber = _ovoNumber,
                        Provenance = OrganisationProvenance.FromWegwijsSearch("uri//12")
                    }));

            _sut = new OrganisationProvider(
                Mock.Of<ILogger<OrganisationProvider>>(),
                _dataVlaanderenProviderMock.Object,
                _wegwijsProviderMock.Object);
        }

        [Fact]
        public void ThenTheDataVlaanderenGetOrganisationFailedMessageWasCalled()
        {
            _dataVlaanderenProviderMock.Verify(provider => provider.GetOrganisationFailedMessage(), Times.Once);
        }

        [Fact]
        public void ThenWegWijsGetOrganisationWasCalled()
        {
            _wegwijsProviderMock.Verify(provider => provider.GetOrganisationAsync(_ovoNumber), Times.Once);
        }
    }
}
