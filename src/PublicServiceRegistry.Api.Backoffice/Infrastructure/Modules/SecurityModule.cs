namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.Modules
{
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Security;

    public class SecurityModule : Module
    {
        private readonly IConfiguration _configuration;

        public SecurityModule(IConfiguration configuration)
            => _configuration = configuration;

        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterInstance<IConfigureOptions<OpenIdConnectConfiguration>>(
                    new ConfigureFromConfigurationOptions<OpenIdConnectConfiguration>(
                        _configuration.GetSection(OpenIdConnectConfiguration.Section)))
                .SingleInstance();

            containerBuilder
                .RegisterInstance<IConfigureOptions<OIDCAuthAcmConfiguration>>(
                    new ConfigureFromConfigurationOptions<OIDCAuthAcmConfiguration>(
                        _configuration.GetSection(OIDCAuthAcmConfiguration.Section)))
                .SingleInstance();
        }
    }
}
