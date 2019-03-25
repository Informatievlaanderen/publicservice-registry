namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector.Testing;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using FluentAssertions.Execution;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.EntityFrameworkCore;
    using Xunit.Sdk;

    public static class ConnectedProjectionTestSpecificationExtensions
    {
        public static ConnectedProjectionScenario<BackofficeContext> Scenario(this ConnectedProjection<BackofficeContext> projection) =>
            new ConnectedProjectionScenario<BackofficeContext>(Resolve.WhenEqualToHandlerMessageType(projection.Handlers));

        private static async Task<object[]> AllRecords(this BackofficeContext context)
        {
            var records = new List<object>();
            records.AddRange(await context.PublicServiceList.ToArrayAsync());
            records.AddRange(await context.PublicServiceLabelList.ToArrayAsync());
            return records.ToArray();
        }

        public static async Task Expect(
            this ConnectedProjectionScenario<BackofficeContext> scenario,
            params object[] records)
        {
            var database = Guid.NewGuid().ToString("N");

            var specification = scenario.Verify(async context =>
            {
                var comparisonConfig = new ComparisonConfig { MaxDifferences = 5 };
                var comparer = new CompareLogic(comparisonConfig);
                var actualRecords = await context.AllRecords();
                var result = comparer.Compare(
                    actualRecords,
                    records);

                return result.AreEqual
                    ? VerificationResult.Pass()
                    : VerificationResult.Fail(result.CreateDifferenceMessage(actualRecords, records));
            });

            using (var context = CreateContextFor(database))
            {
                var projector = new ConnectedProjector<BackofficeContext>(specification.Resolver);
                foreach (var message in specification.Messages)
                {
                    var envelope = new Envelope(message, new Dictionary<string, object>()).ToGenericEnvelope();
                    await projector.ProjectAsync(context, envelope);

                    await context.SaveChangesAsync();
                }
            }

            using (var context = CreateContextFor(database))
            {
                var result = await specification.Verification(context, CancellationToken.None);

                if (result.Failed)
                    throw specification.CreateFailedScenarioExceptionFor(result);
            }
        }

        private static BackofficeContext CreateContextFor(string database)
        {
            var options = new DbContextOptionsBuilder<BackofficeContext>()
                .UseInMemoryDatabase(database)
                .EnableSensitiveDataLogging()
                .Options;

            return new BackofficeContext(options);
        }

        private static XunitException CreateFailedScenarioExceptionFor(this ConnectedProjectionTestSpecification<BackofficeContext> specification, VerificationResult result)
        {
            var title = string.Empty;
            var exceptionMessage = new StringBuilder()
                .AppendLine(title)
                .AppendTitleBlock("Given", specification.Messages, Formatters.NamedJsonMessage)
                .Append(result.Message);

            return new XunitException(exceptionMessage.ToString());
        }

        public static async Task Assert(this ConnectedProjectionTestSpecification<BackofficeContext> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            var options = new DbContextOptionsBuilder<BackofficeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new BackofficeContext(options))
            {
                context.Database.EnsureCreated();

                foreach (var message in specification.Messages)
                {
                    await new ConnectedProjector<BackofficeContext>(specification.Resolver)
                        .ProjectAsync(context, message);

                    await context.SaveChangesAsync();
                }

                var result = await specification.Verification(context, CancellationToken.None);
                if (result.Failed)
                    throw new AssertionFailedException(result.Message);
            }
        }
    }
}
