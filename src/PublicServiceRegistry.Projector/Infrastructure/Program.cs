namespace PublicServiceRegistry.Projector.Infrastructure
{
    using System.Security.Cryptography.X509Certificates;
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        private static readonly DevelopmentCertificate DevelopmentCertificate =
            new DevelopmentCertificate(
                "api.dienstverlening-test.basisregisters.vlaanderen.pfx",
                "dienstverlening!");

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();


        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .UseDefaultForApi<Startup>(
                    new ProgramOptions
                    {
                        Hosting =
                        {
                            HttpPort = 8006,
                            HttpsPort = 8007,
                            HttpsCertificate = DevelopmentCertificate.ToCertificate
                        },
                        Logging =
                        {
                            WriteTextToConsole = false,
                            WriteJsonToConsole = false
                        },
                        Runtime =
                        {
                            CommandLineArgs = args
                        }
                    });
    }
}
