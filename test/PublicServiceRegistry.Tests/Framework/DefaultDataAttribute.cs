namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using AutoFixture;
    using AutoFixture.Xunit2;

    public class DefaultDataAttribute : AutoDataAttribute
    {
        public DefaultDataAttribute() : this(() => new Fixture()) { }

        protected DefaultDataAttribute(Func<IFixture> fixtureFactory)
            : base(() => fixtureFactory().Customize(new WithDefaults())) { }
    }

    public class UnCustomizedDataAttribute : AutoDataAttribute
    {
        public UnCustomizedDataAttribute() : this(() => new Fixture()) { }

        protected UnCustomizedDataAttribute(Func<IFixture> fixtureFactory)
            : base(fixtureFactory) { }
    }

    public class InlineDefaultDataAttribute : InlineAutoDataAttribute
    {
        public InlineDefaultDataAttribute(params object[] values) : base(new DefaultDataAttribute(), values) { }
    }
}
