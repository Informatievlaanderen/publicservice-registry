namespace PublicServiceRegistry.PublicService.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("StageWasAddedToLifeCycle")]
    [EventDescription("Er werd een periode toegekend aan een levensloopfase van de dienstverlening.")]
    public class StageWasAddedToLifeCycle
    {
        public string PublicServiceId { get; }
        public int LifeCycleStageId { get; }
        public string LifeCycleStageType { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }

        public StageWasAddedToLifeCycle(PublicServiceId publicServiceId,
            int lifeCycleStageId,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
            LifeCycleStageType = lifeCycleStageType;
            From = period.Start;
            To = period.End;
        }

        [JsonConstructor]
        private StageWasAddedToLifeCycle(
            string publicServiceId,
            int lifeCycleStageId,
            string lifeCycleStageType,
            DateTime? from,
            DateTime? to) :
            this(
                new PublicServiceId(publicServiceId),
                lifeCycleStageId,
                PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageType),
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to)))
        {
        }
    }
}
