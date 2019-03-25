namespace PublicServiceRegistry.OrafinUpload
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Projections.Backoffice;

    public class OrafinUploadModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly ILoggerFactory _loggerFactory;

        public OrafinUploadModule(
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
            builder.RegisterModule(new BackofficeModule(_configuration, _services, _loggerFactory));

            builder
                .RegisterInstance(_configuration)
                .As<IConfiguration>()
                .SingleInstance();

            var ftpOptions = new ConfigureFromConfigurationOptions<FtpConfiguration>(_configuration.GetSection(FtpConfiguration.SectionName));
            builder
                .RegisterInstance<IConfigureOptions<FtpConfiguration>>(ftpOptions)
                .SingleInstance();

            builder
                .RegisterType<UploadService>()
                .SingleInstance();

            builder
                .RegisterType<FtpConnection>()
                .As<IFtpConnection>();

            builder
                .RegisterType<OrafinFormatter>()
                .As<IOrafinFormatter>();

            builder
                .RegisterType<PublicServiceRepository>()
                .As<IPublicServiceRepository>();

            builder.Populate(_services);
        }
    }
}
