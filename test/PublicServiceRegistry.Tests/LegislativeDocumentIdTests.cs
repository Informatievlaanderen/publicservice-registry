namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class LegislativeDocumentIdTests
    {
        [Fact]
        public void WithValidId()
        {
            var legislativeDocumentId = new LegislativeDocumentId("123634647");
            legislativeDocumentId.Should().Be(new LegislativeDocumentId("123634647"));
        }

        [Fact]
        public void WithNonNumericals()
        {
            Assert.Throws<ArgumentException>(() => new LegislativeDocumentId("1A"));
            Assert.Throws<ArgumentException>(() => new LegislativeDocumentId("11!"));
            Assert.Throws<ArgumentException>(() => new LegislativeDocumentId("alphas"));
            Assert.Throws<ArgumentException>(() => new LegislativeDocumentId("11A1|!38@11"));
        }

        [Fact]
        public void WithEmptyId()
        {
            Assert.Throws<ArgumentException>(() => new LegislativeDocumentId(""));
        }

        [Fact]
        public void WithNullId()
        {
            Assert.Throws<ArgumentNullException>(() => new LegislativeDocumentId(null));
        }
    }
}
