namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector.Testing;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using PublicServiceList;
    using PublicServiceRegistry.PublicService.Events;
    using Xunit;

    public class PublicServiceListTests
    {
        private readonly Fixture _fixture;

        private static readonly DateTime Today = DateTime.Now.Date;
        private static readonly DateTime Tomorrow = DateTime.Now.AddDays(1).Date;
        private static readonly DateTime Yesterday = DateTime.Now.AddDays(-1).Date;

        private static readonly LifeCycleStagePeriod PeriodValidAlways = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo());
        private static readonly LifeCycleStagePeriod PeriodOverlappingWithToday = new LifeCycleStagePeriod(new ValidFrom(LocalDate.FromDateTime(Today)), new ValidTo(LocalDate.FromDateTime(Today)));
        private static readonly LifeCycleStagePeriod PeriodOverlappingWithTomorrow = new LifeCycleStagePeriod(new ValidFrom(LocalDate.FromDateTime(Tomorrow)), new ValidTo(LocalDate.FromDateTime(Tomorrow)));
        private static readonly LifeCycleStagePeriod PeriodOverlappingWithYesterday = new LifeCycleStagePeriod(new ValidFrom(LocalDate.FromDateTime(Yesterday)), new ValidTo(LocalDate.FromDateTime(Yesterday)));

        public PublicServiceListTests()
        {
            _fixture = new Fixture();
            _fixture.CustomizePublicServiceWasRegistered();
        }

        [Fact]
        public async Task WhenPublicServiceWasRegistered()
        {
            var projection = new PublicServiceListProjections(new ClockProviderStub(DateTime.Now));
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

            return new PublicServiceListProjections(new ClockProviderStub(DateTime.Now))
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

            return new PublicServiceListProjections(new ClockProviderStub(DateTime.Now))
                .Scenario()
                .Given(publicServiceWasRegistered, publicServiceWasRemoved)
                .Expect();
        }

        [Fact]
        public Task ShowsLifeCycleStageWhenLifeCycleStageWasAddedThatOverlapsWithToday()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodValidAlways),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = "Active",
                    CurrentLifeCycleStageId = 1,
                });
        }

        [Fact]
        public Task ShowsLifeCycleStageWhenPeriodWasChangedToOverlapWithToday()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithTomorrow),
                new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(1), PeriodOverlappingWithToday),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = "Active",
                    CurrentLifeCycleStageId = 1,
                    CurrentLifeCycleStageEndsAt = LocalDate.FromDateTime(Today),
                });
        }

        [Fact]
        public Task DoesNotShowLifeCycleStageWhenLifeCycleStageWasAddedThatDoesNotOverlapWithToday()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithTomorrow),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = null,
                    CurrentLifeCycleStageId = null,
                });
        }

        [Fact]
        public Task ClearsLifeCycleStageWhenActiveLifeCycleStageNoLongerOverlapsWithToday()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodValidAlways),
                new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(1), PeriodOverlappingWithTomorrow),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = null,
                    CurrentLifeCycleStageId = null,
                });
        }

        [Fact]
        public Task DoesNotClearLifeCycleStageWhenAnotherLifeCycleStageNoLongerOverlapsWithToday()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithToday),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), LifeCycleStageType.PhasingOut, PeriodOverlappingWithTomorrow),
                new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(2), PeriodOverlappingWithYesterday),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = "Active",
                    CurrentLifeCycleStageId = 1,
                    CurrentLifeCycleStageEndsAt = LocalDate.FromDateTime(Today),
                });
        }

        [Fact]
        public Task ClearsLifeCycleStageWhenLastDayHasPassed()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithToday),
                new ClockHasTicked(Tomorrow),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = null,
                    CurrentLifeCycleStageId = null,
                    CurrentLifeCycleStageEndsAt = null,
                });
        }

        [Fact]
        public Task SetsNextLifeCycleStageWhenDayHasPassed()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithToday),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), LifeCycleStageType.PhasingOut, PeriodOverlappingWithTomorrow),
                new ClockHasTicked(Tomorrow),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = "PhasingOut",
                    CurrentLifeCycleStageId = 2,
                    CurrentLifeCycleStageEndsAt = LocalDate.FromDateTime(Tomorrow),
                });
        }

        [Fact]
        public Task DoesNotSetNextLifeCycleStageWhenDayHasPassedWhenThatLifeCycleStageWasRemoved()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.Active, PeriodOverlappingWithToday),
                new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), LifeCycleStageType.PhasingOut, PeriodOverlappingWithTomorrow),
                new LifeCycleStageWasRemoved(publicServiceId, LifeCycleStageId.FromNumber(2)),
                new ClockHasTicked(Tomorrow),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = null,
                    CurrentLifeCycleStageId = null,
                    CurrentLifeCycleStageEndsAt = null,
                });
        }

        [Fact]
        public Task WhenIpdcCodeWasSet()
        {
            var clockProviderStub = new ClockProviderStub(Today);

            var publicServiceId = new PublicServiceId("DVR000000001");
            var events = new object[]
            {
                new PublicServiceWasRegistered(publicServiceId, new PublicServiceName("Test"), PrivateZoneId.Unregistered),
                new IpdcCodeWasSet(publicServiceId, new IpdcCode("1234")),
            };

            return new PublicServiceListProjections(clockProviderStub)
                .Scenario()
                .Given(events)
                .Expect(new PublicServiceListItem
                {
                    Name = "Test",
                    PublicServiceId = "DVR000000001",
                    CurrentLifeCycleStageType = null,
                    CurrentLifeCycleStageId = null,
                    CurrentLifeCycleStageEndsAt = null,
                    IpdcCode = "1234"
                });
        }
    }
}
