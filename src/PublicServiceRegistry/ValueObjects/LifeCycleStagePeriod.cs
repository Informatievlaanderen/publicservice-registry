namespace PublicServiceRegistry
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;
    using NodaTime;
    using PublicService.Exceptions;

    public class LifeCycleStagePeriod : ValueObject<LifeCycleStagePeriod>
    {
        public ValidFrom Start { get; }
        public ValidTo End { get; }

        public bool HasFixedStart => !Start.IsInfinite;
        public bool HasFixedEnd => !End.IsInfinite;

        public LifeCycleStagePeriod(
            [JsonProperty("start")] ValidFrom start,
            [JsonProperty("end")] ValidTo end)
        {
            var endDate = end.Date;
            var startDate = start.Date;
            if (endDate < startDate)
                throw new StartDateCannotBeAfterEndDateException();

            Start = start;
            End = end;
        }

        protected override IEnumerable<object> Reflect()
        {
            yield return Start;
            yield return End;
        }
        public bool OverlapsWith(LifeCycleStagePeriod period)
        {
            if (period == null)
                return false;

            var periodEndDate = period.End.Date;
            var startDate = Start.Date;
            if (periodEndDate < startDate)
                return false;

            var endDate = End.Date;
            var periodStartDate = period.Start.Date;
            if (endDate < periodStartDate)
                return false;

            return true;
        }

        public bool OverlapsWith(LocalDate date)
            => OverlapsWith(
                new LifeCycleStagePeriod(
                    new ValidFrom(date),
                    new ValidTo(date)));

        public override string ToString() => $"{Start} => {End}";
    }
}
