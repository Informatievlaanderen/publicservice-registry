namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLabelValue : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LabelValue>(c => c.FromFactory(() => new LabelValue(fixture.Create<string>())));
    }
}
