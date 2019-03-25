namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class PublicServiceNameTests
    {
        [Fact]
        public void WithValidName()
        {
            var publicServiceName = new PublicServiceName("Uitreiken identiteitskaart");
            publicServiceName.Should().Be(new PublicServiceName("Uitreiken identiteitskaart"));
        }

        [Fact]
        public void WithEmptyName()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceName(""));
        }

        [Fact]
        public void WithNullName()
        {
            Assert.Throws<ArgumentException>(() => new PublicServiceName(null));
        }
    }
}
