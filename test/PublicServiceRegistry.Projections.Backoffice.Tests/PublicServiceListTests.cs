namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector.Testing;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using FluentAssertions;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using PublicServiceList;
    using PublicServiceRegistry.PublicService.Events;
    using Xunit;

    public class PublicServiceListTests
    {
        private readonly Fixture _fixture;

        public PublicServiceListTests()
        {
            _fixture = new Fixture();
            _fixture.CustomizePublicServiceWasRegistered();
        }

        [Fact]
        public async Task WhenPublicServiceWasRegistered()
        {
            var projection = new PublicServiceListProjections();
            var resolver = ConcurrentResolve.WhenEqualToHandlerMessageType(projection.Handlers);

            var publicServiceId = PublicServiceId.FromNumber(123);
            var publicServiceWasRegistered = new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Ophaling huisvuil"), PrivateZoneId.Unregistered);

            await new ConnectedProjectionScenario<BackofficeContext>(resolver)
                .Given(
                    new Envelope<PublicServiceWasRegistered>(new Envelope(
                        publicServiceWasRegistered, new Dictionary<string, object>())))
                .Verify(async context =>
                {
                    var publicService = await context.PublicServiceList.FirstAsync(a => a.PublicServiceId == publicServiceId);

                    publicService.PublicServiceId.Should().Be("DVR000000123");
                    publicService.Name.Should().Be("Ophaling huisvuil");
                    publicService.CompetentAuthorityCode.Should().BeNullOrEmpty();
                    publicService.CompetentAuthorityName.Should().BeNullOrEmpty();
                    publicService.ExportToOrafin.Should().Be(false);

                    return VerificationResult.Pass();
                })
                .Assert();
        }

        [Fact]
        public Task WhenPublicServiceWasRegisteredALot()
        {
            var random = new Random();
            var data =
                _fixture.CreateMany<PublicServiceWasRegistered>(random.Next(1, 100))
                .Select(registeredPublicService =>
                {
                    var expected = new PublicServiceListItem
                    {
                        PublicServiceId = registeredPublicService.PublicServiceId,
                        Name = registeredPublicService.Name,
                        CompetentAuthorityCode = null,
                        CompetentAuthorityName = null,
                        ExportToOrafin = false,
                    };

                    return new
                    {
                        events = registeredPublicService,
                        expected
                    };
                }).ToList();

            return new PublicServiceListProjections()
                .Scenario()
                .Given(data.Select(d => d.events))
                .Expect(data
                    .Select(d => d.expected)
                    .Cast<object>()
                    .ToArray());
        }

        [Fact]
        public Task WhenPublicServiceWasRemoved()
        {
            var publicServiceWasRegistered = _fixture.Create<PublicServiceWasRegistered>();
            var publicServiceWasRemoved = new PublicServiceWasRemoved(new PublicServiceId(publicServiceWasRegistered.PublicServiceId), new ReasonForRemoval("because"));

            return new PublicServiceListProjections()
                .Scenario()
                .Given(publicServiceWasRegistered, publicServiceWasRemoved)
                .Expect();
        }
    }

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendTitleBlock(
            this StringBuilder builder,
            string title,
            Action<StringBuilder> addContent)
        {
            builder.AppendLine($"{title}:");
            addContent(builder);
            builder.AppendLine();

            return builder;
        }

        public static StringBuilder AppendTitleBlock(
            this StringBuilder builder,
            string title,
            string content)
        {
            return builder.AppendTitleBlock(title, b => b.AppendLine(content));
        }

        public static StringBuilder AppendTitleBlock<T>(
            this StringBuilder builder,
            string title,
            IEnumerable<T> collection,
            Func<T, string> formatter)
        {
            return builder.AppendTitleBlock(title, b => b.AppendLines(collection, formatter));
        }

        public static StringBuilder AppendLines<T>(
            this StringBuilder builder,
            IEnumerable<T> collection,
            Func<T, string> formatter)
        {
            foreach (var item in collection)
                builder.AppendLine(formatter(item));

            return builder;
        }
    }

    public static class Formatters
    {
        public static string NamedJsonMessage<T>(T message) => $"{message.GetType().Name} - {JsonConvert.SerializeObject(message, Formatting.Indented)}";
    }

    public static class ComparisonResultExtensions
    {
        public static string CreateDifferenceMessage(this ComparisonResult result, object[] actual, object[] expected)
        {
            var message = new StringBuilder();

            message
                .AppendTitleBlock("Expected", expected, Formatters.NamedJsonMessage)
                .AppendTitleBlock("But", actual, Formatters.NamedJsonMessage)
                .AppendTitleBlock("Difference", result.DifferencesString.Trim());

            return message.ToString();
        }
    }
}
