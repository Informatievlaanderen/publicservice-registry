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
        public string LifeCycleStage { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }

        public StageWasAddedToLifeCycle(PublicServiceId publicServiceId,
            int id,
            LifeCycleStage lifeCycleStage,
            LifeCycleStagePeriod period)
        {
            PublicServiceId = publicServiceId;
            Id = id;
            LifeCycleStage = lifeCycleStage;
            From = period.Start;
            To = period.End;
        }

        [JsonConstructor]
        private StageWasAddedToLifeCycle(
            string publicServiceId,
            int id,
            string lifeCycleStage,
            DateTime? from,
            DateTime? to) :
            this(
                new PublicServiceId(publicServiceId),
                id,
                PublicServiceRegistry.LifeCycleStage.Parse(lifeCycleStage),
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to)))
        {
        }
    }
}
