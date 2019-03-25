namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Formatters.Csv;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.AspNetCore;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Configuration;
    using ExceptionHandlers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;
    using Modules;
    using Security;
    using SqlStreamStore;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>Represents the startup process for the application.</summary>
    public class Startup
    {
        private IContainer _applicationContainer;

        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        /// <summary>Configures services for the application.</summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var openIdConfiguration = _configuration.GetSection(OpenIdConnectConfiguration.Section).Get<OpenIdConnectConfiguration>();

            if (string.IsNullOrEmpty(openIdConfiguration.ClientId) ||
                string.IsNullOrEmpty(openIdConfiguration.ClientSecret))
                throw new OpenIdConnectCredentialsNotProvided();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters =
                        new PublicServiceRegistryTokenValidationParameters(openIdConfiguration);
                });

            services
                .ConfigureDefaultForApi<Startup>(
                    (provider, description) => new Info
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = "Basisregisters Vlaanderen Public Service Registry API",
                        Description = GetApiLeadingText(description),
                        Contact = new Contact
                        {
                            Name = "Informatie Vlaanderen",
                            Email = "informatie.vlaanderen@vlaanderen.be",
                            Url = "https://legacy.basisregisters.vlaanderen"
                        }
                    },
                    new[] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name },
                    corsOrigins: _configuration.GetSection("Cors").GetChildren().Select(c => c.Value).ToArray(),
                    corsHeaders: new[] { PublicServiceHeaderNames.LastObservedPosition },
                    corsExposedHeaders: new[] { PublicServiceHeaderNames.LastObservedPosition },
                    configureMvcBuilder: x => x
                        .AddMvcOptions(options => options.OutputFormatters.Add(new CsvOutputFormatter(new CsvFormatterOptions())))
                        .AddFormatterMappings(mappings => mappings.SetMediaTypeMappingForFormat(CsvOutputFormatter.Format, MediaTypeHeaderValue.Parse(CsvOutputFormatter.ContentType))));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApiModule(_configuration, services, _loggerFactory));
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider,
            MsSqlStreamStore streamStore,
            ApiDataDogToggle datadogToggle,
            ApiDebugDataDogToggle debugDataDogToggle)
        {
            StartupHelpers.EnsureSqlStreamStoreSchema<Startup>(streamStore, loggerFactory);

            if (datadogToggle.FeatureEnabled)
            {
                if (debugDataDogToggle.FeatureEnabled)
                    StartupHelpers.SetupSourceListener(serviceProvider.GetRequiredService<TraceSource>());

                app.UseDataDogTracing(
                    serviceProvider.GetRequiredService<TraceSource>(),
                    _configuration["DataDog:ServiceName"],
                    pathToCheck => pathToCheck != "/");
            }

            app.UseDefaultForApi(new StartupOptions
            {
                ApplicationContainer = _applicationContainer,
                ServiceProvider = serviceProvider,
                HostingEnvironment = env,
                ApplicationLifetime = appLifetime,
                LoggerFactory = loggerFactory,
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = groupName => $"Basisregisters Vlaanderen - Dienstverleningsregister API {groupName}",
                    CustomExceptionHandlers = new []
                    {
                        new GetOrganisationFailedHandler()
                    }
                },
                MiddlewareHooks =
                {
                    AfterApiExceptionHandler = x => x.UseAuthentication(),
                    AfterMiddleware = x => x.UseMiddleware<AddNoCacheHeadersMiddleware>(),
                }
            });
        }

        private static string GetApiLeadingText(ApiVersionDescription description)
            => $"Momenteel leest u de documentatie voor versie {description.ApiVersion} van de Basisregisters Vlaanderen Public Service Registry API{string.Format(description.IsDeprecated ? ", **deze API versie is niet meer ondersteund * *." : ".")}";
    }
}