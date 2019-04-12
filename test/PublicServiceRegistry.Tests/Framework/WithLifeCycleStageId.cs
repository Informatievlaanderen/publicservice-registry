namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLifeCycleStageId : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LifeCycleStageId>(c => c.FromFactory(() => LifeCycleStageId.FromNumber(fixture.Create<int>())));
    }
}