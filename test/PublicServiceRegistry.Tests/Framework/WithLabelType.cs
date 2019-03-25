namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLabelType : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LabelType>(c => c.FromFactory<int>(value => LabelType.All[value % LabelType.All.Length]));
    }
}
