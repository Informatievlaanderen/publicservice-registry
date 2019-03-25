namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        private static class DevelopmentCertificate
        {
            internal const string Name = "api.dienstverlening-test.basisregisters.vlaanderen.pfx";
            internal const string Key = "dienstverlening!";
        }

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .UseDefaultForApi<Startup>(
                    httpPort: 2090,
                    httpsPort: 2443,
                    httpsCertificate: () => new X509Certificate2(DevelopmentCertificate.Name, DevelopmentCertificate.Key),
                    commandLineArgs: args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                        config.AddUserSecrets<Startup>();
                });
    }
}
