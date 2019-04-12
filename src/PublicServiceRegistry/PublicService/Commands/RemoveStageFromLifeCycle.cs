namespace PublicServiceRegistry.PublicService.Commands
{
    public class RemoveStageFromLifeCycle
    {
        public PublicServiceId PublicServiceId { get; }

        public LifeCycleStageId LifeCycleStageId { get; }

        public RemoveStageFromLifeCycle(
            PublicServiceId publicServiceId,
            LifeCycleStageId lifeCycleStageId)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
        }
    }
}
