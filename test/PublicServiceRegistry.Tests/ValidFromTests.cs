namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using NodaTime;
    using NodaTime.Extensions;
    using Xunit;

    public class ValidFromTests
    {
        [Fact]
        public void WithSameDate_IsEqualTo()
        {
            new ValidFrom().Should().Be(new ValidFrom());
            new ValidFrom(2017, 1, 1).Should().Be(new ValidFrom(2017, 1, 1));
        }

        [Fact]
        public void WithDifferentDate_IsNotEqualTo()
        {
            new ValidFrom().Should().NotBe(new ValidFrom(2017, 1, 1));
            new ValidFrom(2018, 1, 1).Should().NotBe(new ValidFrom(2017, 1, 1));
        }

        [Fact]
        public void NullDate_IsSmallerThan_AnyDate()
        {
            (new ValidFrom(null) < new ValidFrom(LocalDate.MinIsoValue)).Should().BeTrue();
            (new ValidFrom(null) <= new ValidFrom(LocalDate.MinIsoValue)).Should().BeTrue();

            (new ValidFrom(LocalDate.MaxIsoValue) > new ValidFrom(null)).Should().BeTrue();
            (new ValidFrom(LocalDate.MaxIsoValue) >= new ValidFrom(null)).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_UseRegularGreaterThan()
        {
            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now)) > new ValidFrom(LocalDate.FromDateTime(DateTime.Now.AddDays(-1)))).Should().BeTrue();
            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now)) >= new ValidFrom(LocalDate.FromDateTime(DateTime.Now.AddDays(-1)))).Should().BeTrue();

            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now.AddDays(-1))) < new ValidFrom(LocalDate.FromDateTime(DateTime.Now))).Should().BeTrue();
            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now.AddDays(-1))) <= new ValidFrom(LocalDate.FromDateTime(DateTime.Now))).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_AreEqualTo()
        {
            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now.Date)) == new ValidFrom(LocalDate.FromDateTime(DateTime.Now.Date))).Should().BeTrue();

            new ValidFrom(LocalDate.FromDateTime(DateTime.Now.Date)).Equals(new ValidFrom(LocalDate.FromDateTime(DateTime.Now.Date))).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_AreNotEqualTo_NullDates()
        {
            (new ValidFrom(LocalDate.FromDateTime(DateTime.Now.Date)) == new ValidFrom()).Should().BeFalse();
            new ValidFrom(LocalDate.FromDateTime(DateTime.Now)).Equals(new ValidFrom()).Should().BeFalse();
        }

        [Fact]
        public void NullDates_AreEqualTo_NullDates()
        {
            (new ValidFrom() == new ValidFrom()).Should().BeTrue();
            new ValidFrom().Equals(new ValidFrom()).Should().BeTrue();
        }
    }
}
