namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class LifeCycleStageIdTests
    {
        [Fact]
        public void ImplicitConversionReturnsExpectedResult()
        {
            int result = LifeCycleStageId.FromNumber(1);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ExplicitConversionReturnsExpectedResult()
        {
            Assert.Equal(1, (int)LifeCycleStageId.FromNumber(1));
        }

        [Fact]
        public void CannotCreateFromNumberLowerThan1()
        {
            Assert.Throws<ArgumentException>(() => LifeCycleStageId.FromNumber(0));
            Assert.Throws<ArgumentException>(() => LifeCycleStageId.FromNumber(-1));
        }

        [Fact]
        public void ZeroReturns0()
        {
            Assert.Equal(0, LifeCycleStageId.Zero());
        }

        [Fact]
        public void NextReturnsExpectedResult()
        {
            Assert.Equal(LifeCycleStageId.FromNumber(1), LifeCycleStageId.Zero().Next());
            Assert.Equal(LifeCycleStageId.FromNumber(3), LifeCycleStageId.FromNumber(2).Next());
        }
    }
}
