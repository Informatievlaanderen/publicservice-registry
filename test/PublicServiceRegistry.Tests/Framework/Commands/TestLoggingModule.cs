namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using System;
    using Autofac;
    using Destructurama;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Debugging;

    public class TestLoggingModule : Module
    {
        public TestLoggingModule(IServiceCollection services)
        {
            SelfLog.Enable(Console.WriteLine);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentUserName()
                .Destructure.JsonNetTypes()
                .CreateLogger();

            services.AddLogging(l => l.AddSerilog(Log.Logger));
        }
    }
}
