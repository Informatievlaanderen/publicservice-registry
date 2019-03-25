namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithPublicServiceName : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<PublicServiceName>(c => c.FromFactory(() => new PublicServiceName("Naam dienstverlening " + fixture.Create<int>())));
    }
}
