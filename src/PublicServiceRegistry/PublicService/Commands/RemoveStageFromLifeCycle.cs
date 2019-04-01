namespace PublicServiceRegistry.PublicService.Commands
{
    public class RemoveStageFromLifeCycle
    {
        public PublicServiceId PublicServiceId { get; }

        public int LifeCycleStageId { get; }

        public RemoveStageFromLifeCycle(
            PublicServiceId publicServiceId,
            int lifeCycleStageId)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
        }
    }
}
