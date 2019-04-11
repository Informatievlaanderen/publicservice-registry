namespace PublicServiceRegistry.Tests
{
    using System;
    using FluentAssertions;
    using NodaTime;
    using Xunit;

    public class ValidToTests
    {
        [Fact]
        public void WithSameDate_IsEqualTo()
        {
            new ValidTo().Should().Be(new ValidTo());
            new ValidTo(2017, 1, 1).Should().Be(new ValidTo(2017, 1, 1));
        }

        [Fact]
        public void WithDifferentDate_IsNotEqualTo()
        {
            new ValidTo().Should().NotBe(new ValidTo(2017, 1, 1));
            new ValidTo(2018, 1, 1).Should().NotBe(new ValidTo(2017, 1, 1));
        }

        [Fact]
        public void NullDate_IsGreaterThan_AnyDate()
        {
            (new ValidTo(null) > new ValidTo(LocalDate.MaxIsoValue)).Should().BeTrue();
            (new ValidTo(null) >= new ValidTo(LocalDate.MaxIsoValue)).Should().BeTrue();

            (new ValidTo(LocalDate.MaxIsoValue) < new ValidTo(null)).Should().BeTrue();
            (new ValidTo(LocalDate.MaxIsoValue) <= new ValidTo(null)).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_UseRegularGreaterThan()
        {
            (new ValidTo(LocalDate.FromDateTime(DateTime.Now)) > new ValidTo(LocalDate.FromDateTime(DateTime.Now.AddDays(-1)))).Should().BeTrue();
            (new ValidTo(LocalDate.FromDateTime(DateTime.Now)) >= new ValidTo(LocalDate.FromDateTime(DateTime.Now.AddDays(-1)))).Should().BeTrue();

            (new ValidTo(LocalDate.FromDateTime(DateTime.Now.AddDays(-1))) < new ValidTo(LocalDate.FromDateTime(DateTime.Now))).Should().BeTrue();
            (new ValidTo(LocalDate.FromDateTime(DateTime.Now.AddDays(-1))) <= new ValidTo(LocalDate.FromDateTime(DateTime.Now))).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_AreEqualTo()
        {
            (new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date)) == new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date))).Should().BeTrue();

            new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date)).Equals(new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date))).Should().BeTrue();
        }

        [Fact]
        public void RegularDates_AreNotEqualTo_NullDates()
        {
            (new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date)) == new ValidTo()).Should().BeFalse();
            new ValidTo(LocalDate.FromDateTime(DateTime.Now.Date)).Equals(new ValidTo()).Should().BeFalse();
        }

        [Fact]
        public void NullDates_AreEqualTo_NullDates()
        {
            (new ValidTo() == new ValidTo()).Should().BeTrue();
            new ValidTo().Equals(new ValidTo()).Should().BeTrue();
        }
    }
}
