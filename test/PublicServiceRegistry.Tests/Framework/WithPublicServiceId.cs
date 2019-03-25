namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithPublicServiceId : ICustomization
    {
        private readonly int? _fixedId;

        public WithPublicServiceId(int? fixedId = null) => _fixedId = fixedId;

        public void Customize(IFixture fixture) =>
            fixture.Customize<PublicServiceId>(c => c.FromFactory(() => PublicServiceId.FromNumber(_fixedId ?? fixture.Create<int>())));
    }
}
