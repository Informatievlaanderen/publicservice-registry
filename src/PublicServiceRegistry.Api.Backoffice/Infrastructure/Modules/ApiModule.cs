namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.Modules
{
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore.Autofac;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Projections.Backoffice;
    using PublicService;

    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly ILoggerFactory _loggerFactory;

        public ApiModule(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _services = services;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            var eventSerializerSettings = EventsJsonSerializerSettingsProvider.CreateSerializerSettings();

            containerBuilder
                .RegisterType<DvrCodeGenerator>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterModule(new SecurityModule(_configuration))
                .RegisterModule(new DataDogModule(_configuration))
                .RegisterModule(new BackofficeModule(_configuration, _services, _loggerFactory))
                .RegisterModule(new EventHandlingModule(typeof(DomainAssemblyMarker).Assembly, eventSerializerSettings))
                .RegisterModule(new EnvelopeModule())
                .RegisterModule(new CommandHandlingModule(_configuration));

            containerBuilder.Populate(_services);
        }
    }
}
