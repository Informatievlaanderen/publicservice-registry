namespace PublicServiceRegistry.OrafinUpload
{
    using System;
    using System.IO;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Destructurama;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class ApplicationBuilder
    {
        public IServiceProvider Build()
        {
            var services = new ServiceCollection();
            var configuration = CreateApplicationConfiguration();

            var serviceProvider = services
                .AddLogging(builder => ConfigureLogging(configuration, builder))
                .BuildServiceProvider();

            var containerBuilder = new ContainerBuilder();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            containerBuilder
                .RegisterModule(new OrafinUploadModule(configuration, services, loggerFactory));

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        private static IConfiguration CreateApplicationConfiguration()
        {
            Console.WriteLine("Configure {0}.", Program.ServiceDescription);

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private static void ConfigureLogging(
            IConfiguration configuration,
            ILoggingBuilder logging)
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentUserName()
                .Destructure.JsonNetTypes();

            var logger = Log.Logger = loggerConfiguration.CreateLogger();

            logging.AddSerilog(logger);
        }
    }
}
