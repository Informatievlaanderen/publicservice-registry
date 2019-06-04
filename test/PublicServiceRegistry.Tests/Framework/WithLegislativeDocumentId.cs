namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLegislativeDocumentId : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LegislativeDocumentId>(c =>
                c.FromFactory(() =>
                    new LegislativeDocumentId(fixture.Create<int>().ToString())));
    }
}
