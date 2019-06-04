namespace PublicServiceRegistry.Projector.Infrastructure.Modules
{
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.LastChangedList;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore.Autofac;
    using Be.Vlaanderen.Basisregisters.Projector;
    using Be.Vlaanderen.Basisregisters.Projector.Modules;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PublicServiceRegistry.Infrastructure;
    using PublicServiceRegistry.Projections.Backoffice;
    using PublicServiceRegistry.Projections.Backoffice.PublicService;
    using PublicServiceRegistry.Projections.Backoffice.PublicServiceLabelList;
    using PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle;
    using PublicServiceRegistry.Projections.Backoffice.PublicServiceList;
    using PublicServiceRegistry.Projections.LastChangedList;
    using LastChangedListContextMigrationFactory = PublicServiceRegistry.Projections.LastChangedList.LastChangedListContextMigrationFactory;

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

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterModule(new LoggingModule(_configuration, _services))
                .RegisterModule(new DataDogModule(_configuration));

            RegisterProjectionSetup(builder);

            builder.Populate(_services);
        }

        private void RegisterProjectionSetup(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                    new EventHandlingModule(
                        typeof(DomainAssemblyMarker).Assembly,
                        EventsJsonSerializerSettingsProvider.CreateSerializerSettings()))

                .RegisterModule<EnvelopeModule>()

                .RegisterEventstreamModule(_configuration)

                .RegisterModule<ProjectorModule>();

            RegisterBackofficeProjections(builder);
            RegisterLastChangedProjections(builder);
        }

        private void RegisterBackofficeProjections(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                    new BackofficeModule(
                        _configuration,
                        _services,
                        _loggerFactory));

            builder
                .RegisterType<ClockProvider>()
                .As<IClockProvider>();

            builder
                .RegisterProjectionMigrator<BackofficeContextMigrationFactory>(
                    _configuration,
                    _loggerFactory)

                .RegisterProjections<PublicServiceProjections, BackofficeContext>()
                .RegisterProjections<PublicServiceListProjections, BackofficeContext>(x => new PublicServiceListProjections(x.Resolve<IClockProvider>()))
                .RegisterProjections<PublicServiceLabelListProjections, BackofficeContext>()
                .RegisterProjections<PublicServiceLifeCycleListProjections, BackofficeContext>();
        }

        private void RegisterLastChangedProjections(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                new LastChangedListModule(
                    _configuration.GetConnectionString("LastChangedList"),
                    _configuration["DataDog:ServiceName"],
                    _services,
                    _loggerFactory));

            builder
                .RegisterProjectionMigrator<LastChangedListContextMigrationFactory>(
                    _configuration,
                    _loggerFactory)

                .RegisterProjections<LastChangedListProjections, LastChangedListContext>();
        }
    }
}
