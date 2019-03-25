namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using System;
    using Autofac;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublicService;
    using PublicService.Providers;

    public class OrganisationMocker
    {
        private readonly AutofacBasedTest _source;

        public OrganisationMocker(AutofacBasedTest source)
        {
            _source = source;
            MockLogging();
        }

        private void MockLogging()
        {
            _source.ModifyContainer(builder =>
            {
                builder
                    .RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();

                builder
                    .RegisterType<Logger<OrganisationProvider>>()
                    .As<ILogger<OrganisationProvider>>();
            });
        }

        public void MockOrganisationProvider(OvoNumber ovoNumber, Organisation organisation)
        {
            _source.ModifyContainer(builder =>
            {
                var organisationProviderMock = new Mock<IOrganisationProvider>();
                organisationProviderMock
                    .Setup(provider => provider.GetOrganisationAsync(ovoNumber))
                    .ReturnsAsync(organisation);

                builder
                    .RegisterInstance(organisationProviderMock.Object)
                    .As<IOrganisationProvider>();
            });
        }

        public void MockOrganisationProviderFailure(OvoNumber ovoNumber, Exception exception)
        {
            _source.ModifyContainer(builder =>
            {
                var organisationProviderMock = new Mock<IOrganisationProvider>();
                organisationProviderMock
                    .Setup(provider => provider.GetOrganisationAsync(ovoNumber))
                    .Throws(exception);

                builder
                    .RegisterInstance(organisationProviderMock.Object)
                    .As<IOrganisationProvider>();
            });
        }
    }
}
