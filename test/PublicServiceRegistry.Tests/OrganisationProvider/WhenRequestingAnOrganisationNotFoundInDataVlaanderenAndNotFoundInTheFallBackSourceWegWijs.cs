namespace PublicServiceRegistry.Tests.OrganisationProvider
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublicService;
    using PublicService.Exceptions;
    using PublicService.Providers;
    using PublicService.Providers.OrganisationProviderSources;
    using Xunit;

    public class WhenRequestingAnOrganisationNotFoundInDataVlaanderenAndNotFoundInTheFallBackSourceWegWijs
    {
        private Mock<IDataVlaanderenProvider> _dataVlaanderenProviderMock;
        private Mock<IWegwijsProvider> _wegwijsProviderMock;

        private Action _act;

        public WhenRequestingAnOrganisationNotFoundInDataVlaanderenAndNotFoundInTheFallBackSourceWegWijs()
        {
            SetupContext();
        }

        private void SetupContext()
        {
            var ovoNumber = new OvoNumber("OVO000015");

            _dataVlaanderenProviderMock = new Mock<IDataVlaanderenProvider>();
            _wegwijsProviderMock = new Mock<IWegwijsProvider>();

            _dataVlaanderenProviderMock
                .Setup(provider => provider.GetOrganisationAsync(ovoNumber))
                .ReturnsAsync(() => Optional<Organisation>.Empty);

            _dataVlaanderenProviderMock
                .Setup(provider => provider.GetOrganisationNotFoundMessage())
                .Returns("data.vlaanderen: not found");

            _wegwijsProviderMock
                .Setup(provider => provider.GetOrganisationAsync(ovoNumber))
                .ReturnsAsync(() => Optional<Organisation>.Empty);

            _wegwijsProviderMock
                .Setup(provider => provider.GetOrganisationNotFoundMessage())
                .Returns("wegwijs: not found");

            var sut = new OrganisationProvider(
                Mock.Of<ILogger<OrganisationProvider>>(),
                _dataVlaanderenProviderMock.Object,
                _wegwijsProviderMock.Object);

            _act = () => sut.GetOrganisationAsync(ovoNumber).GetAwaiter().GetResult();
        }

        [Fact]
        public void ThenGetOrganisationFailedExceptionIsThrown()
        {
            _act.Should().Throw<GetOrganisationFailed>();
        }

        [Fact]
        public void ThenTheDataVlaanderenNotFoundMessageWasCalled()
        {
            try
            {
                _act();
            }
            catch {}

            _dataVlaanderenProviderMock.Verify(provider => provider.GetOrganisationNotFoundMessage(), Times.Once);
        }

        [Fact]
        public void ThenTheWegwijsOrganisationNotFoundMessageWasCalled()
        {
            try
            {
                _act();
            }
            catch {}

            _wegwijsProviderMock.Verify(provider => provider.GetOrganisationNotFoundMessage(), Times.Once);
        }
    }
}
