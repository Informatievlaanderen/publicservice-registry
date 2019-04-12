namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class RemoveLifeCycleStageTests : AutofacBasedTest
    {
        public RemoveLifeCycleStageTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory]
        [DefaultData]
        public void CannotRemoveLifeCycleStageOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LifeCycleStagePeriod period)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.PhasingOut, period),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new RemoveStageFromLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1)))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void RemoveLifeCycleStage(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStagePeriod period)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.PhasingOut, period))
                    .When(new RemoveStageFromLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1)))
                    .Then(publicServiceId,
                        new LifeCycleStageWasRemoved(publicServiceId, LifeCycleStageId.FromNumber(1))));
        }

        [Theory]
        [DefaultData]
        public void CantRemoveARemovedLifeCycleStage(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStagePeriod period)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), LifeCycleStageType.PhasingOut, period),
                        new LifeCycleStageWasRemoved(publicServiceId, LifeCycleStageId.FromNumber(1)))
                    .When(new RemoveStageFromLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1)))
                    .Throws(new LifeCycleStageWithGivenIdNotFound(1)));
        }
    }
}
