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

    public class AddStageToLifeCycleTests : AutofacBasedTest
    {
        public AddStageToLifeCycleTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)  { }

        [Theory]
        [DefaultData]
        public void CannotSetOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LifeCycleStage lifeCycleStage,
            LifeCycleStagePeriod period)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStage, period))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WithValidData(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStage lifeCycleStage,
            LifeCycleStagePeriod period)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStage, period))
                .Then(publicServiceId,
                    new StageWasAddedToLifeCycle(publicServiceId, 1, lifeCycleStage, period)));
        }

        [Theory]
        [DefaultData]
        public void WithPeriodThatHasChanged(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStage lifeCycleStage,
            LifeCycleStagePeriod period,
            LifeCycleStagePeriod newPeriod)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, 1, lifeCycleStage, period))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStage, newPeriod))
                .Then(publicServiceId,
                    new StageWasAddedToLifeCycle(publicServiceId, 2, lifeCycleStage, newPeriod)));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleCannotHaveOverlappingPeriods(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStage lifeCycleStage1,
            LifeCycleStage lifeCycleStage2)
        {
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2020, 1, 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2019, 1, 1), new ValidTo(2021, 1, 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, 1, lifeCycleStage1, period1))
                .When(new AddStageToLifeCycle(publicServiceId, lifeCycleStage2, period2))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }

        [Theory]
        [DefaultData]
        public void LifeCycleOverlapsTakesChangesIntoAccount(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LifeCycleStage lifeCycleStage)
        {
            var period1 = new LifeCycleStagePeriod(new ValidFrom(2018, 1, 1), new ValidTo(2018, 1, 1));
            var period2 = new LifeCycleStagePeriod(new ValidFrom(2025, 1, 1), new ValidTo(2025, 1, 1));
            // Combining period3 with period4 will cause an overlap.
            var period3 = new LifeCycleStagePeriod(new ValidFrom(2019, 1, 1), new ValidTo(2021, 1, 1));
            var period4 = new LifeCycleStagePeriod(new ValidFrom(2019, 1, 1), new ValidTo(2021, 1, 1));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new StageWasAddedToLifeCycle(publicServiceId, 1, lifeCycleStage, period1),
                    new StageWasAddedToLifeCycle(publicServiceId, 2, lifeCycleStage, period2),
                    new PeriodOfLifeCycleStageWasChanged(publicServiceId, 1, period3))
                .When(new ChangePeriodOfLifeCycleStage(publicServiceId, 2, period4))
                .Throws(new LifeCycleCannotHaveOverlappingPeriods()));
        }
    }
}
