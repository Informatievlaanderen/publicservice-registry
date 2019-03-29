namespace PublicServiceRegistry.PublicService.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("StageWasAddedToLifeCycle")]
    [EventDescription("Er werd een periode toegekend aan een levensfase van de dienstverlening.")]
    public class StageWasAddedToLifeCycle
    {
        public string PublicServiceId { get; }
        public int Id { get; }
        public string LifeCycleStageType { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }

        public StageWasAddedToLifeCycle(PublicServiceId publicServiceId,
            int id,
            LifeCycleStageType lifeCycleStageType,
            LifeCycleStagePeriod period)
        {
            PublicServiceId = publicServiceId;
            Id = id;
            LifeCycleStageType = lifeCycleStageType;
            From = period.Start;
            To = period.End;
        }

        [JsonConstructor]
        private StageWasAddedToLifeCycle(
            string publicServiceId,
            int id,
            string lifeCycleStageType,
            DateTime? from,
            DateTime? to) :
            this(
                new PublicServiceId(publicServiceId),
                id,
                PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageType),
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to)))
        {
        }
    }
}
