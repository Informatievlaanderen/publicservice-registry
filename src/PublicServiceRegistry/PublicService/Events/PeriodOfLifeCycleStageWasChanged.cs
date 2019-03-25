namespace PublicServiceRegistry.PublicService.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("PeriodOfLifeCycleStageWasChanged")]
    [EventDescription("De periode van een levensfase van de dienstverlening werd gewijzigd.")]
    public class PeriodOfLifeCycleStageWasChanged
    {
        public string PublicServiceId { get; }
        public int Id { get; }
        public DateTime? From { get; }
        public DateTime? To { get; }

        public PeriodOfLifeCycleStageWasChanged(
            PublicServiceId publicServiceId,
            int id,
            LifeCycleStagePeriod period)
        {
            PublicServiceId = publicServiceId;
            Id = id;
            From = period.Start;
            To = period.End;
        }

        [JsonConstructor]
        private PeriodOfLifeCycleStageWasChanged(
            string publicServiceId,
            int id,
            DateTime? from,
            DateTime? to) :
            this(
                new PublicServiceId(publicServiceId),
                id,
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to))) { }
    }
}
