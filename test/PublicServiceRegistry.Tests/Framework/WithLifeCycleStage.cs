namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithLifeCycleStage : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<LifeCycleStageType>(c => c.FromFactory<int>(value => LifeCycleStageType.All[value % LifeCycleStageType.All.Length]));
    }
}
