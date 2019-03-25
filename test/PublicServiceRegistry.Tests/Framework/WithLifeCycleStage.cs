namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLifeCycleStage : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LifeCycleStage>(c => c.FromFactory<int>(value => LifeCycleStage.All[value % LifeCycleStage.All.Length]));
    }
}
