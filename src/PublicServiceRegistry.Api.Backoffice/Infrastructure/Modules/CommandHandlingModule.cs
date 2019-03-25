namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.Modules
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using PublicServiceRegistry.Infrastructure;
    using PublicServiceRegistry.PublicService.Providers;
    using PublicServiceRegistry.PublicService.Providers.OrganisationProviderSources;

    public class CommandHandlingModule : Module
    {
        private readonly IConfiguration _configuration;

        public CommandHandlingModule(IConfiguration configuration)
            => _configuration = configuration;

        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterModule<RepositoriesModule>();

            containerBuilder
                .RegisterType<ConcurrentUnitOfWork>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<DataVlaanderenProvider>()
                .As<IDataVlaanderenProvider>();

            containerBuilder
                .RegisterType<WegwijsProvider>()
                .As<IWegwijsProvider>();

            containerBuilder
                .RegisterType<OrganisationProvider>()
                .As<IOrganisationProvider>();

            containerBuilder
                .RegisterEventstreamModule(_configuration);

            CommandHandlerModules.Register(containerBuilder);

            containerBuilder
                .RegisterType<CommandHandlerResolver>()
                .As<ICommandHandlerResolver>();
        }
    }
}
