namespace PublicServiceRegistry.Tests
{
    using System;
    using Framework.Assertions;
    using Xunit;
    using AutoFixture;
    using AutoFixture.Idioms;

    public class LifeCycleStageTests
    {
        private readonly Fixture _fixture;

        private readonly string[] _knownValues;

        public LifeCycleStageTests()
        {
            _fixture = new Fixture();
            _knownValues = Array.ConvertAll(LifeCycleStage.All, type => type.ToString());
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
            ).Verify(typeof(LifeCycleStage));
        }

        [Fact]
        public void ActiveReturnsExpectedResult()
        {
            Assert.Equal("Active", LifeCycleStage.Active);
        }

        [Fact]
        public void UnderDevelopmentReturnsExpectedResult()
        {
            Assert.Equal("UnderDevelopment", LifeCycleStage.UnderDevelopment);
        }

        [Fact]
        public void PhasingOutReturnsExpectedResult()
        {
            Assert.Equal("PhasingOut", LifeCycleStage.PhasingOut);
        }

        [Fact]
        public void StoppedReturnsExpectedResult()
        {
            Assert.Equal("Stopped", LifeCycleStage.Stopped);
        }

        [Fact]
        public void AllReturnsExpectedResult()
        {
            Assert.Equal(
                new[]
                {
                    LifeCycleStage.Active,
                    LifeCycleStage.UnderDevelopment,
                    LifeCycleStage.PhasingOut,
                    LifeCycleStage.Stopped,
                },
                LifeCycleStage.All);
        }

        [Fact]
        public void ToStringReturnsExpectedResult()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var sut = LifeCycleStage.Parse(value);
            var result = sut.ToString();

            Assert.Equal(value, result);
        }

        [Fact]
        public void ParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LifeCycleStage.Parse(null));
        }

        [Fact]
        public void ParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            Assert.NotNull(LifeCycleStage.Parse(value));
        }

        [Fact]
        public void ParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            Assert.Throws<FormatException>(() => LifeCycleStage.Parse(value));
        }

        [Fact]
        public void TryParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LifeCycleStage.TryParse(null, out _));
        }

        [Fact]
        public void TryParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var result = LifeCycleStage.TryParse(value, out var parsed);
            Assert.True(result);
            Assert.NotNull(parsed);
            Assert.Equal(value, parsed.ToString());
        }

        [Fact]
        public void TryParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            var result = LifeCycleStage.TryParse(value, out var parsed);
            Assert.False(result);
            Assert.Null(parsed);
        }

        [Fact]
        public void CanParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LifeCycleStage.CanParse(null));
        }

        [Fact]
        public void CanParseReturnsExpectedResultWhenValueIsUnknown()
        {
            var value = _fixture.Create<string>();
            var result = LifeCycleStage.CanParse(value);
            Assert.False(result);
        }

        [Fact]
        public void CanParseReturnsExpectedResultWhenValueIsWellKnown()
        {
            var value = _knownValues[new Random().Next(0, _knownValues.Length)];
            var result = LifeCycleStage.CanParse(value);
            Assert.True(result);
        }
    }
}
