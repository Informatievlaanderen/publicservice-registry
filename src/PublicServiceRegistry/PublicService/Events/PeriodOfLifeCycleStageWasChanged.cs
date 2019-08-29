namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;
    using NodaTime;

    [EventName("PeriodOfLifeCycleStageWasChanged")]
    [EventDescription("De periode van een levensloopfase van de dienstverlening werd gewijzigd.")]
    public class PeriodOfLifeCycleStageWasChanged
    {
        public string PublicServiceId { get; }
        public int LifeCycleStageId { get; }
        public LocalDate? From { get; }
        public LocalDate? To { get; }

        public PeriodOfLifeCycleStageWasChanged(
            PublicServiceId publicServiceId,
            LifeCycleStageId lifeCycleStageId,
            LifeCycleStagePeriod period)
        {
            PublicServiceId = publicServiceId;
            LifeCycleStageId = lifeCycleStageId;
            From = period.Start;
            To = period.End;
        }

        [JsonConstructor]
        private PeriodOfLifeCycleStageWasChanged(
            string publicServiceId,
            int lifeCycleStageId,
            LocalDate? from,
            LocalDate? to) :
            this(
                new PublicServiceId(publicServiceId),
                PublicServiceRegistry.LifeCycleStageId.FromNumber(lifeCycleStageId),
                new LifeCycleStagePeriod(new ValidFrom(from), new ValidTo(to))) { }
    }
}
