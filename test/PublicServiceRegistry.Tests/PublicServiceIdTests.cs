namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class PublicServiceIdTests
    {
        [Fact]
        public void WithValidId()
        {
            var publicServiceId = new PublicServiceId("DVR000000001");
            publicServiceId.Should().Be(new PublicServiceId("DVR000000001"));
        }

        [Fact]
        public void WithMaxId()
        {
            var publicServiceId = new PublicServiceId("DVR999999999");
            publicServiceId.Should().Be(new PublicServiceId("DVR999999999"));
        }

        [Fact]
        public void WithIdNotStartingWithDVR()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceId("000000001"));
        }

        [Fact]
        public void WithIdWithWrongLength()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceId("DVR0000000001"));
        }

        [Fact]
        public void WithIdWithWrongFormat()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceId("DVRXXX000001"));
        }

        [Fact]
        public void WithZeroAsNumber()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceId("DVR000000000"));
        }

        [Fact]
        public void WithNullOrEmptyOrWhitespaceId()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceId(string.Empty));
            Assert.Throws<ArgumentException>(() => new PublicServiceId(""));
            Assert.Throws<ArgumentException>(() => new PublicServiceId(" "));
            Assert.Throws<ArgumentException>(() => new PublicServiceId("   "));
            Assert.Throws<ArgumentException>(() => new PublicServiceId(null));
        }
    }
}
