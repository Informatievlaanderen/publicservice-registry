namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class AddStageToLifeCycleTests : AutofacBasedTest
    {
        public AddStageToLifeCycleTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)  { }

        [Theory]
        [DefaultData]
        public void CannotSetOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStageType, period))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WithValidData(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStageType, period))
                .Then(publicServiceId,
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period)));
        }

        [Theory]
        [DefaultData]
        public void WithPeriodThatHasChanged(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period,
            LifeCycleStagePeriod newPeriod)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStageType, newPeriod))
                .Then(publicServiceId,
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), lifeCycleStageType, newPeriod)));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleCannotHaveOverlappingPeriods(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStage1,
            LifeCycleStageType lifeCycleStage2)
        {
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2020, LifeCycleStageId.FromNumber(1), 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2019, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2021, LifeCycleStageId.FromNumber(1), 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStage1, period1))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStage2, period2))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleOverlapsTakesChangesIntoAccount(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStageType lifeCycleStageType)
        {
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2018, LifeCycleStageId.FromNumber(1), 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2025, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2025, LifeCycleStageId.FromNumber(1), 1));
            // Combining period3 with period4 will cause an overlap.
            var period3 = new LifeCycleStagePeriod(new ValidFrom(2019, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2021, LifeCycleStageId.FromNumber(1), 1));
            var period4 = new LifeCycleStagePeriod(new ValidFrom(2019, LifeCycleStageId.FromNumber(1), 1), new ValidTo(2021, LifeCycleStageId.FromNumber(1), 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(1), lifeCycleStageType, period1),
                    new StageWasAddedToLifeCycle(publicServiceId, LifeCycleStageId.FromNumber(2), lifeCycleStageType, period2),
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, LifeCycleStageId.FromNumber(1), period3))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, LifeCycleStageId.FromNumber(2), period4))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }
    }
}
