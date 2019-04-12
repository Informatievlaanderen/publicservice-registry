namespace PublicServiceRegistry.PublicService.Commands
{
    public class ChangePeriodOfLifeCycleStage
    {
        public PublicServiceId PublicServiceId { get; }

        public LifeCycleStageId LifeCycleStageId { get; }

        public LifeCycleStagePeriod LifeCycleStagePeriod { get; }

        public ChangePeriodOfLifeCycleStage(
            PublicServiceId publicServiceId,
            LifeCycleStageId lifeCycleStageId,
            LifeCycleStagePeriod lifeCycleStagePeriod)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
            LifeCycleStagePeriod = lifeCycleStagePeriod;
        }
    }
}
