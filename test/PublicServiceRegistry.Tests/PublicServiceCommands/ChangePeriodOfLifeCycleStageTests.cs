namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class ChangePeriodOfLifeCycleStageTests : AutofacBasedTest
    {
        public ChangePeriodOfLifeCycleStageTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)  { }

        [Theory]
        [DefaultData]
        public void CannotChangePeriodOfLifeCycleStageOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period,
            LifeCycleStageId lifeCycleStageId)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, lifeCycleStageId, lifeCycleStageType, period),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, lifeCycleStageId, period))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WithValidData(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period,
            LifeCycleStageId lifeCycleStageLocalId)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, lifeCycleStageLocalId, lifeCycleStageType, period))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, lifeCycleStageLocalId, period))
                .Then(publicServiceId,
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, lifeCycleStageLocalId, period)));
        }

        [Theory]
        [DefaultData]
        public void WithPeriodThatHasChangedTwice(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period,
            LifeCycleStagePeriod newPeriod,
            LifeCycleStageId lifeCycleStageLocalId)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, lifeCycleStageLocalId, lifeCycleStageType, period),
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, lifeCycleStageLocalId, period))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, lifeCycleStageLocalId, newPeriod))
                .Then(publicServiceId,
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, lifeCycleStageLocalId, newPeriod)));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleCannotHaveOverlappingPeriods(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType)
        {
            // These two periods are ok because they don't overlap
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2020, 1, 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2020, 1, 2), new ValidTo(2021, 1, 1));
            // Combining period1 with period3 will cause an overlap, however.
            var period3 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2020, 1, 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period1),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), lifeCycleStageType, period2))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, LifeCycleStageId.FromNumber(2), period3))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleOverlapsTakesChangesIntoAccount(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType)
        {
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2018, 1, 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2020, 1, 2), new ValidTo(2022, 1, 1));
            // Combining period2 with period3 will cause an overlap.
            var period3 = new LifeCycleStagePeriod(new ValidFrom(2019, 1, 1), new ValidTo(2021, 1, 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period1),
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(1), period2))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStageType, period3))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleStageDoesNotOverlapWhenChangingItsOwnPeriod(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType)
        {
            // These two periods are ok because they don't overlap
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2020, 1, 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2020, 1, 2), new ValidTo(2021, 1, 1));
            // Changing period1 to period3 for '1' will not cause an overlap
            var period3 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2019, 1, 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period1),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), lifeCycleStageType, period2))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, LifeCycleStageId.FromNumber(1), period3))
                .Then(publicServiceId,
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(1), period3)));
        }
    }
}
