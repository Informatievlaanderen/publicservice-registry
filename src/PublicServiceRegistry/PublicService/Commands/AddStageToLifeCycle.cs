namespace PublicServiceRegistry.PublicService.Commands
{
    public class AddStageToLifeCycle
    {
        public PublicServiceId PublicServiceId { get; }

        public LifeCycleStageType LifeCycleStageType { get; }
        public LifeCycleStagePeriod LifeCycleStagePeriod { get; }

        public AddStageToLifeCycle(
            PublicServiceId publicServiceId,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod lifeCycleStagePeriod)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageType = lifeCycleStageType;
            LifeCycleStagePeriod = lifeCycleStagePeriod;
        }
    }
}
