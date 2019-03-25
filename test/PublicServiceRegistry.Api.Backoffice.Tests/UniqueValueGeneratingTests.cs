namespace PublicServiceRegistry.Api.Backoffice.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Framework;
    using Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Projections.Backoffice;
    using PublicService.Requests;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Categories;

    [Collection(DockerTestCollection.Name)]
    public class UniqueValueGeneratingTests : IAsyncLifetime
    {
        private static string AppsettingsJson => Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "appsettings.json");

        private readonly ITestOutputHelper _output;
        private readonly ISqlServerDatabase _database;

        public UniqueValueGeneratingTests(DockerFixture fixture, ITestOutputHelper output)
        {
            _output = output;
            _database = fixture.GenerateDatabase(Guid.NewGuid().ToString("N"));
        }

        [Fact(Skip = "Trying to fix build")]
        [IntegrationTest]
        public async Task GenerateUniqueNumber()
        {
            var configuration = SetupConfiguration(new ConfigurationBuilder()).Build();
            await RunMigrations(configuration);

            var client = CreateClient(CreateServer(), configuration);
            var client2 = CreateClient(CreateServer(), configuration);

            var registerServiceRequest =
                JsonConvert.SerializeObject(
                    new RegisterPublicServiceRequest
                    {
                        Naam = "Uitreiken identiteitskaart"
                    });

            var numberOfCalls = 50;
            var tasks = new Task<HttpResponseMessage>[numberOfCalls];

            for (var i = 0; i < numberOfCalls; i++)
            {
                tasks[i] =
                    i % 2 == 0
                        ? client
                            .PostAsync(
                                "/v1/dienstverleningen",
                                new StringContent(registerServiceRequest, Encoding.UTF8, "application/json"))
                        : client2
                            .PostAsync(
                                "/v1/dienstverleningen",
                                new StringContent(registerServiceRequest, Encoding.UTF8, "application/json"));
            }

            var result = await Task<IDictionary<string, string>>.Factory.ContinueWhenAll(tasks, data =>
            {
                Task.WaitAll(data);

                var results = new ConcurrentDictionary<string, string>();

                foreach (var task1 in data)
                {
                    var task = task1;
                    task.Result.StatusCode.Should().Be(StatusCodes.Status202Accepted);

                    var etag = task.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var location = task.Result.Headers.Location;

                    results.TryAdd(etag, location.ToString());
                }

                return results;
            });

            var count = result.Count;
            var keyCount = result.Keys.Distinct().Count();
            var valueCount = result.Values.Distinct().Count();

            _output.WriteLine($"Number of calls: {count}");
            _output.WriteLine($"Results: {count}");
            _output.WriteLine($"Distinct keys: {keyCount}");
            _output.WriteLine($"Distinct values: {valueCount}");

            _output.WriteLine($"Lowest key: {result.Keys.Min()}");
            _output.WriteLine($"Highest key: {result.Keys.Max()}");

            _output.WriteLine($"Lowest value: {result.Values.Min()}");
            _output.WriteLine($"Highest value: {result.Values.Max()}");

            count.Should().Be(numberOfCalls);
            keyCount.Should().Be(numberOfCalls);
            valueCount.Should().Be(numberOfCalls);
        }

        private static async Task RunMigrations(IConfigurationRoot configuration)
        {
            var migrator = new BackofficeContextMigrationFactory().CreateMigrator(configuration, new LoggerFactory());
            await migrator.MigrateAsync(CancellationToken.None);
        }

        private IConfigurationBuilder SetupConfiguration(IConfigurationBuilder builder)
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

            return builder;
        }

        private static HttpClient CreateClient(TestServer server, IConfiguration configuration)
        {
            var client = server.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", configuration.GetValue<string>("TestJwtToken"));

            return client;
        }

        private TestServer CreateServer() =>
            new TestServer(
                new WebHostBuilder()
                    .ConfigureLogging(builder => builder.AddProvider(new XunitLoggerProvider(_output)))
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration(builder => SetupConfiguration(builder)));

        public Task InitializeAsync() => _database.CreateDatabase();

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
