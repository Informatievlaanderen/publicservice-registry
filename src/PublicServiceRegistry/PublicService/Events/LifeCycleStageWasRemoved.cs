namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("LifeCycleStageWasRemoved")]
    [EventDescription("De levensloopfase werd verwijderd.")]
    public class LifeCycleStageWasRemoved
    {
        public string PublicServiceId { get; }
        public int LifeCycleStageId { get; }

        public LifeCycleStageWasRemoved(
            PublicServiceId publicServiceId,
            int lifeCycleStageId)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
        }

        [JsonConstructor]
        private LifeCycleStageWasRemoved(
            string publicServiceId,
            int lifeCycleStageId) :
            this(
                new PublicServiceId(publicServiceId),
                lifeCycleStageId)
        { }
    }
}
