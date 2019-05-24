namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class IpdcCodeTests
    {
        [Fact]
        public void WithValidCode()
        {
            var ipdcCode = new IpdcCode("1111");
            ipdcCode.Should().Be(new IpdcCode("1111"));
        }

        [Fact]
        public void WithIncorrectLength()
        {
            Assert.Throws<ArgumentException>(() => new IpdcCode("1"));
            Assert.Throws<ArgumentException>(() => new IpdcCode("11"));
            Assert.Throws<ArgumentException>(() => new IpdcCode("111"));
            Assert.Throws<ArgumentException>(() => new IpdcCode("11111"));
        }

        [Fact]
        public void WithAllZeroes()
        {
            Assert.Throws<ArgumentException>(() => new IpdcCode("0000"));
        }

        [Fact]
        public void WithAlphas()
        {
            Assert.Throws<ArgumentException>(() => new IpdcCode("111A"));
        }


        [Fact]
        public void WithEmptyCode()
        {
            Assert.Throws<ArgumentException>(() => new IpdcCode(""));
        }

        [Fact]
        public void WithNullCode()
        {
            Assert.Throws<ArgumentNullException>(() => new IpdcCode(null));
        }
    }
}
