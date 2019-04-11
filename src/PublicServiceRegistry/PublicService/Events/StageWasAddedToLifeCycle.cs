namespace PublicServiceRegistry.PublicService.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;
    using NodaTime;

    [EventName("StageWasAddedToLifeCycle")]
    [EventDescription("Er werd een periode toegekend aan een levensloopfase van de dienstverlening.")]
    public class StageWasAddedToLifeCycle
    {
        public string PublicServiceId { get; }
        public int LifeCycleStageId { get; }
        public string LifeCycleStageType { get; }
        public LocalDate? From { get; }
        public LocalDate? To { get; }

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
            LocalDate? from,
            LocalDate? to) :
            this(
                new PublicServiceId(publicServiceId),
                lifeCycleStageId,
                PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageType),
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to)))
        {
        }
    }
}
