namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using PublicService.Exceptions;
    using Xunit;

    public class LifeCycleStagePeriodTests
    {
        [Fact]
        public void LifeCycleStagePeriodsOverlapWhenTheyHaveTheSameBeginAndTheSameEnd()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenItsBeginAndEndFallWithinTheOtherLifeCycleStagePeriod()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2018, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenTheOtherLifeCycleStagePeriodsBeginAndEndFallWithinItself()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2018, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenItsEndFallsBetweenTheOtherLifeCycleStagePeriodsBeginAndEnd()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2018, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenItsBeginFallsBetweenTheOtherLifeCycleStagePeriodsBeginAndEnd()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2017, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 01)), new ValidTo(new DateTime(2018, 01, 01)));

            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenItHasNoEndAndItsStartIsBeforeTheOtherLifeCycleStagePeriodsStart()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo());
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 02)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }


        [Fact]
        public void ALifeCycleStagePeriodOverlapsWhenItHasNoStartAndItsEndIsAfterTheOtherLifeCycleStagePeriodsStart()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo(new DateTime(2015, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2014, 01, 02)), new ValidTo(new DateTime(2016, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void ALifeCycleStagePeriodDoesNotOverlapWhenItHasNoStartAndItsStartIsAfterTheOtherLifeCycleStagePeriodsStart()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo(new DateTime(2015, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 02)), new ValidTo(new DateTime(2017, 12, 31)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeFalse();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeFalse();
        }

        [Fact]
        public void ALifeCycleStagePeriodDoesNotOverlapWhenTheOtherLifeCycleStagePeriodsStartsAfterItsEndLifeCycleStagePeriod()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2016, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 02)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeFalse();
        }

        [Fact]
        public void ALifeCycleStagePeriodDoesNotOverlapWhenTheOtherLifeCycleStagePeriodsEndsBeforeItsStartLifeCycleStagePeriod()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2015, 01, 01)), new ValidTo(new DateTime(2016, 01, 01)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 02)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeFalse();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeFalse();
        }

        [Fact]
        public void SomeoneDidNotBelieveMe()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo());
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2016, 01, 02)), new ValidTo(new DateTime(2017, 01, 01)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void SomeoneStillDidntBelieveMe()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo());
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo());

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void Nope()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo(new DateTime(2000, 1, 1)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(1990, 1, 1)), new ValidTo());

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void StillNot()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo(new DateTime(2000, 1, 1)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(2010, 1, 1)), new ValidTo());

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeFalse();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeFalse();
        }

        [Fact]
        public void AlmostGivingUp()
        {
            var lifeCycleStagePeriod1 = new LifeCycleStagePeriod(new ValidFrom(), new ValidTo(new DateTime(2000, 1, 1)));
            var lifeCycleStagePeriod2 = new LifeCycleStagePeriod(new ValidFrom(new DateTime(1900, 1, 1)), new ValidTo(new DateTime(2010, 1, 1)));

            lifeCycleStagePeriod1.OverlapsWith(lifeCycleStagePeriod2).Should().BeTrue();
            lifeCycleStagePeriod2.OverlapsWith(lifeCycleStagePeriod1).Should().BeTrue();
        }

        [Fact]
        public void CannotCreateALifeCycleStagePeriodWithStartDateAfterEndDate()
        {
            Assert.Throws<StartDateCannotBeAfterEndDateException>(() => new LifeCycleStagePeriod(new ValidFrom(new DateTime(2000, 1, 2)), new ValidTo(new DateTime(2000, 1, 1))));
        }
    }
}
