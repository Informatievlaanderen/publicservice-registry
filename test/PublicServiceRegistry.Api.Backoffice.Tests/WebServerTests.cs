namespace PublicServiceRegistry.Api.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Framework;
    using Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Projections.Backoffice;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Categories;

    [Collection(DockerTestCollection.Name)]
    public class WebServerTests : IAsyncLifetime
    {
        private readonly ITestOutputHelper _helper;

        private readonly ISqlServerDatabase _database;

        // Once we get to external dependencies,
        // we might want to use something like
        // http://geeklearning.io/a-different-approach-to-test-your-asp-net-core-application/ .

        private static string AppsettingsJson => Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "appsettings.json");

        public WebServerTests(
            DockerFixture fixture,
            ITestOutputHelper helper)
        {
            _helper = helper;
            _database = fixture.GenerateDatabase(Guid.NewGuid().ToString("N"));
        }

        [Fact(Skip = "Docker tests are broken in CircleCI for now")]
        [IntegrationTest]
        public async Task TheServerWorks()
        {
            var testServer = new TestServer(
                new WebHostBuilder()
                    .ConfigureLogging(builder => builder.AddProvider(new XunitLoggerProvider(_helper)))
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration(SetupConfiguration)
                    .ConfigureServices(services =>
                    {
                        var tempProvider = services.BuildServiceProvider();
                        var configuration = tempProvider.GetRequiredService<IConfiguration>();
                        var loggerFactory = tempProvider.GetRequiredService<ILoggerFactory>();
                        RunMigrations(configuration, loggerFactory).GetAwaiter().GetResult();
                    }));

            var client = testServer.CreateClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.StartsWith("Welcome to the Basisregisters Vlaanderen Public Service Api", responseString);
        }

        [Fact(Skip = "Docker tests are broken in CircleCI for now")]
        [IntegrationTest]
        public async Task RoutingWorks()
        {
            var testServer = new TestServer(
                new WebHostBuilder()
                    .ConfigureLogging(builder => builder.AddProvider(new XunitLoggerProvider(_helper)))
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration(SetupConfiguration)
                    .ConfigureServices(services =>
                    {
                        var tempProvider = services.BuildServiceProvider();
                        var configuration = tempProvider.GetRequiredService<IConfiguration>();
                        var loggerFactory = tempProvider.GetRequiredService<ILoggerFactory>();
                        RunMigrations(configuration, loggerFactory).GetAwaiter().GetResult();
                    }));

            var client = testServer.CreateClient();

            var response = await client.GetAsync("/v1/dienstverleningen");

            response.EnsureSuccessStatusCode();
        }

        public Task InitializeAsync() => _database.CreateDatabase();

        public Task DisposeAsync() => Task.CompletedTask;

        private void SetupConfiguration(IConfigurationBuilder builder)
        {
            var connectionString = _database.CreateConnectionStringBuilder().ConnectionString;

            builder
                .AddJsonFile(AppsettingsJson)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:Events", connectionString },
                    { "ConnectionStrings:BackofficeProjections", connectionString },
                    { "ConnectionStrings:BackofficeProjectionsAdmin", connectionString },
                });
        }

        private static async Task RunMigrations(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            var migrator = new BackofficeContextMigrationFactory().CreateMigrator(configuration, loggerFactory);
            await migrator.MigrateAsync(CancellationToken.None);
        }
    }
}
