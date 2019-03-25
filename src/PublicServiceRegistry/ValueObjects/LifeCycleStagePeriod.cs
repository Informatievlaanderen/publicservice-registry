namespace PublicServiceRegistry
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using PublicService.Exceptions;

    public class LifeCycleStagePeriod : ValueObject<LifeCycleStagePeriod>
    {
        public ValidFrom Start { get; }
        public ValidTo End { get; }

        public bool HasFixedStart => !Start.IsInfinite;
        public bool HasFixedEnd => !End.IsInfinite;

        public LifeCycleStagePeriod(ValidFrom start, ValidTo end)
        {
            var endDate = end.DateTime;
            var startDate = start.DateTime;
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

            var periodEndDate = period.End.DateTime;
            var startDate = Start.DateTime;
            if (periodEndDate < startDate)
            {
                return false;
            }

            var endDate = End.DateTime;
            var periodStartDate = period.Start.DateTime;
            if (endDate < periodStartDate)
            {
                return false;
            }

            return true;
        }

        public bool OverlapsWith(DateTime date)
        {
            return OverlapsWith(
                new LifeCycleStagePeriod(
                    new ValidFrom(date),
                    new ValidTo(date)));
        }

        public override string ToString()
        {
            return $"{Start} => {End}";
        }
    }
}
