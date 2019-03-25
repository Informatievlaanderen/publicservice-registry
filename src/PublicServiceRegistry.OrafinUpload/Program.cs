namespace PublicServiceRegistry.OrafinUpload
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public const string ServiceDescription = "Orafin FTP data delivery service";

        private static IServiceProvider _app;
        private static IServiceProvider Application => _app ?? (_app = new ApplicationBuilder().Build());

        public static void Main(string[] args)
        {
            Console.WriteLine("Start {0}.", ServiceDescription);
            Application
                .GetService<UploadService>()
                .Run();

#if DEBUG // TODO: remove debug output
            Console.WriteLine();
            Console.WriteLine("Hit your any-key to close the application...");
            Console.ReadKey();
#endif
        }
    }
}
