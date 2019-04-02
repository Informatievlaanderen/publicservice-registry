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
    using PublicServiceLifeCycle;
    using PublicServiceList;
    using PublicServiceRegistry.PublicService.Events;
    using Xunit;

    public class PublicServiceLifeCycleListProjectionsTests
    {
        private readonly Fixture _fixture;

        public PublicServiceLifeCycleListProjectionsTests()
        {
            _fixture = new Fixture();
            _fixture.CustomizeLifeCycleStagePeriod();
            _fixture.CustomizeStageWasAddedToLifeCycle();
        }

        [Fact]
        public async Task WhenPublicServiceWasRegistered()
        {
            var projection = new PublicServiceLifeCycleListProjections();
            var resolver = ConcurrentResolve.WhenEqualToHandlerMessageType(projection.Handlers);

            var stageWasAddedToLifeCycle = _fixture.Create<StageWasAddedToLifeCycle>();
            await new ConnectedProjectionScenario<BackofficeContext>(resolver)
                .Given(
                    new Envelope<StageWasAddedToLifeCycle>(new Envelope(stageWasAddedToLifeCycle, new Dictionary<string, object>())))
                .Verify(async context =>
                {
                    var publicService =
                        await context.PublicServiceLifeCycleList.FirstAsync(a =>
                            a.PublicServiceId == stageWasAddedToLifeCycle.PublicServiceId && a.LocalId == stageWasAddedToLifeCycle.Id);

                    publicService.Should().BeEquivalentTo(new PublicServiceLifeCycleItem()
                    {
                        LocalId = stageWasAddedToLifeCycle.Id,
                        PublicServiceId = stageWasAddedToLifeCycle.PublicServiceId,
                        LifeCycleStageType = stageWasAddedToLifeCycle.LifeCycleStageType,
                        From = stageWasAddedToLifeCycle.From,
                        To = stageWasAddedToLifeCycle.To
                    });

                    return VerificationResult.Pass();
                })
                .Assert();
        }

        [Fact]
        public async Task WhenPeriodOfLifeCycleStageWasChanged()
        {
            var projection = new PublicServiceLifeCycleListProjections();
            var resolver = ConcurrentResolve.WhenEqualToHandlerMessageType(projection.Handlers);

            var stageWasAddedToLifeCycle = _fixture.Create<StageWasAddedToLifeCycle>();
            var periodOfLifeCycleStageWasChanged = new PeriodOfLifeCycleStageWasChanged(
                new PublicServiceId(stageWasAddedToLifeCycle.PublicServiceId),
                stageWasAddedToLifeCycle.Id,
                _fixture.Create<LifeCycleStagePeriod>());

        await new ConnectedProjectionScenario<BackofficeContext>(resolver)
                .Given(
                    new Envelope<StageWasAddedToLifeCycle>(new Envelope(stageWasAddedToLifeCycle, new Dictionary<string, object>())),
                    new Envelope<PeriodOfLifeCycleStageWasChanged>(new Envelope(periodOfLifeCycleStageWasChanged, new Dictionary<string, object>())))
                .Verify(async context =>
                {
                    var publicService =
                        await context.PublicServiceLifeCycleList.FirstAsync(a =>
                            a.PublicServiceId == stageWasAddedToLifeCycle.PublicServiceId && a.LocalId == stageWasAddedToLifeCycle.Id);

                    publicService.Should().BeEquivalentTo(new PublicServiceLifeCycleItem()
                    {
                        LocalId = stageWasAddedToLifeCycle.Id,
                        PublicServiceId = stageWasAddedToLifeCycle.PublicServiceId,
                        LifeCycleStageType = stageWasAddedToLifeCycle.LifeCycleStageType,
                        From = periodOfLifeCycleStageWasChanged.From,
                        To = periodOfLifeCycleStageWasChanged.To
                    });

                    return VerificationResult.Pass();
                })
                .Assert();
        }

        [Fact]
        public async Task WhenLifeCycleStageWasRemoved()
        {
            var projection = new PublicServiceLifeCycleListProjections();
            var resolver = ConcurrentResolve.WhenEqualToHandlerMessageType(projection.Handlers);

            var stageWasAddedToLifeCycle = _fixture.Create<StageWasAddedToLifeCycle>();
            var lifeCycleStageWasRemoved = new LifeCycleStageWasRemoved(
                new PublicServiceId(stageWasAddedToLifeCycle.PublicServiceId),
                stageWasAddedToLifeCycle.Id);

            await new ConnectedProjectionScenario<BackofficeContext>(resolver)
                .Given(
                    new Envelope<StageWasAddedToLifeCycle>(new Envelope(stageWasAddedToLifeCycle, new Dictionary<string, object>())),
                    new Envelope<LifeCycleStageWasRemoved>(new Envelope(lifeCycleStageWasRemoved, new Dictionary<string, object>())))
                .Verify(async context =>
                {
                    var publicService =
                        await context.PublicServiceLifeCycleList.FirstOrDefaultAsync(a =>
                            a.PublicServiceId == stageWasAddedToLifeCycle.PublicServiceId && a.LocalId == stageWasAddedToLifeCycle.Id);

                    publicService.Should().BeNull();

                    return VerificationResult.Pass();
                })
                .Assert();
        }

        [Fact]
        public async Task WhenPublicServiceWasRemoved()
        {
            var projection = new PublicServiceLifeCycleListProjections();
            var resolver = ConcurrentResolve.WhenEqualToHandlerMessageType(projection.Handlers);

            var stageWasAddedToLifeCycle = _fixture.Create<StageWasAddedToLifeCycle>();
            var publicServiceWasRemoved = new PublicServiceWasRemoved(
                new PublicServiceId(stageWasAddedToLifeCycle.PublicServiceId),
                new ReasonForRemoval("testing purposes"));

            await new ConnectedProjectionScenario<BackofficeContext>(resolver)
                .Given(
                    new Envelope<StageWasAddedToLifeCycle>(new Envelope(stageWasAddedToLifeCycle, new Dictionary<string, object>())),
                    new Envelope<PublicServiceWasRemoved>(new Envelope(publicServiceWasRemoved, new Dictionary<string, object>())))
                .Verify(async context =>
                {
                    var publicService =
                        await context.PublicServiceLifeCycleList.FirstOrDefaultAsync(a =>
                            a.PublicServiceId == stageWasAddedToLifeCycle.PublicServiceId && a.LocalId == stageWasAddedToLifeCycle.Id);

                    publicService.Should().BeNull();

                    return VerificationResult.Pass();
                })
                .Assert();
        }
    }
}
