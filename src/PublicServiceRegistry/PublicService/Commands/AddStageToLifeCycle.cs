namespace PublicServiceRegistry.PublicService.Commands
{
    public class AddStageToLifeCycle
    {
        public PublicServiceId PublicServiceId { get; }

        public LifeCycleStage LifeCycleStage { get; }
        public LifeCycleStagePeriod LifeCycleStagePeriod { get; }

        public AddStageToLifeCycle(
            PublicServiceId publicServiceId,
            LifeCycleStage lifeCycleStage,
            LifeCycleStagePeriod lifeCycleStagePeriod)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStage = lifeCycleStage;
            LifeCycleStagePeriod = lifeCycleStagePeriod;
        }
    }
}
