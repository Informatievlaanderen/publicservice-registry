namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithPrivateZoneId : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<PrivateZoneId>(c => c.FromFactory(() => new PrivateZoneId("Mijn private zone")));
    }
}
