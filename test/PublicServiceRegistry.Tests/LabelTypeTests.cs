namespace PublicServiceRegistry.Tests
{
    using System;
    using Framework.Assertions;
    using Xunit;
    using AutoFixture;
    using AutoFixture.Idioms;

    public class LabelTypeTests
    {
        private readonly Fixture _fixture;

        private readonly string[] _knownValues;

        public LabelTypeTests()
        {
            _fixture = new Fixture();
            _knownValues = Array.ConvertAll(LabelType.All, type => type.ToString());
        }

        [Fact]
        public void VerifyBehavior()
        {
            _fixture.Customizations.Add(
                new FiniteSequenceGenerator<string>(_knownValues));
            new CompositeIdiomaticAssertion(
                new ImplicitConversionOperatorAssertion<string>(_fixture),
                new EquatableEqualsSelfAssertion(_fixture),
                new EquatableEqualsOtherAssertion(_fixture),
                new EqualityOperatorEqualsSelfAssertion(_fixture),
                new EqualityOperatorEqualsOtherAssertion(_fixture),
                new InequalityOperatorEqualsSelfAssertion(_fixture),
                new InequalityOperatorEqualsOtherAssertion(_fixture),
                new EqualsNewObjectAssertion(_fixture),
                new EqualsNullAssertion(_fixture),
                new EqualsSelfAssertion(_fixture),
                new EqualsOtherAssertion(_fixture),
                new EqualsSuccessiveAssertion(_fixture),
                new GetHashCodeSuccessiveAssertion(_fixture)
            ).Verify(typeof(LabelType));
        }

        [Fact]
        public void IpdcReturnsExpectedResult()
        {
            Assert.Equal("Ipdc", LabelType.Ipdc);
        }

        [Fact]
        public void SubsidieregisterReturnsExpectedResult()
        {
            Assert.Equal("Subsidieregister", LabelType.Subsidieregister);
        }

        [Fact]
        public void AllReturnsExpectedResult()
        {
            Assert.Equal(
                new[]
                {
                    LabelType.Ipdc,
                    LabelType.Subsidieregister,
                },
                LabelType.All);
        }

        [Fact]
        public void ToStringReturnsExpectedResult()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var sut = LabelType.Parse(value);
            var result = sut.ToString();

            Assert.Equal(value, result);
        }

        [Fact]
        public void ParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LabelType.Parse(null));
        }

        [Fact]
        public void ParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            Assert.NotNull(LabelType.Parse(value));
        }

        [Fact]
        public void ParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            Assert.Throws<FormatException>(() => LabelType.Parse(value));
        }

        [Fact]
        public void TryParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LabelType.TryParse(null, out _));
        }

        [Fact]
        public void TryParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var result = LabelType.TryParse(value, out var parsed);
            Assert.True(result);
            Assert.NotNull(parsed);
            Assert.Equal(value, parsed.ToString());
        }

        [Fact]
        public void TryParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            var result = LabelType.TryParse(value, out var parsed);
            Assert.False(result);
            Assert.Null(parsed);
        }

        [Fact]
        public void CanParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LabelType.CanParse(null));
        }

        [Fact]
        public void CanParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            var result = LabelType.CanParse(value);
            Assert.False(result);
        }

        [Fact]
        public void CanParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var result = LabelType.CanParse(value);
            Assert.True(result);
        }
    }
}
