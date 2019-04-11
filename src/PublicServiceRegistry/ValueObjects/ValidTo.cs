namespace PublicServiceRegistry
{
    using System;
    using System.Globalization;
    using NodaTime;

    public struct ValidTo : IEquatable<ValidTo>, IComparable<ValidTo>
    {
        public LocalDate? Date { get; }

        public bool IsInfinite => !Date.HasValue;

        public ValidTo(LocalDate? localDate)
        {
            Date = localDate;
        }

        public ValidTo(int year, int month, int day)
        {
            Date = new LocalDate(year, month, day);
        }

        public static implicit operator LocalDate? (ValidTo validTo)
        {
            return validTo.Date;
        }

        public override string ToString()
        {
            return Date.HasValue ? Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "~";
        }

        public bool IsInFutureOf(LocalDate date, bool inclusive = false)
        {
            return inclusive
                ? this >= new ValidTo(date)
                : this > new ValidTo(date);
        }

        public bool IsInPastOf(LocalDate date, bool inclusive = false)
        {
            return inclusive
                ? this <= new ValidTo(date)
                : this < new ValidTo(date);
        }

        public static bool operator ==(ValidTo left, ValidTo right) => left.Equals(right);

        public static bool operator !=(ValidTo left, ValidTo right) => !(left == right);

        public static bool operator <(ValidTo left, ValidTo right) => left.CompareTo(right) < 0;

        public static bool operator <=(ValidTo left, ValidTo right) => left.CompareTo(right) <= 0;

        public static bool operator >(ValidTo left, ValidTo right) => left.CompareTo(right) > 0;

        public static bool operator >=(ValidTo left, ValidTo right) => left.CompareTo(right) >= 0;

        public int CompareTo(ValidTo other)
        {
            if (!Date.HasValue && !other.Date.HasValue)
                return 0;

            if (!Date.HasValue)
                return 1;

            if (!other.Date.HasValue)
                return -1;

            return Date.Value.CompareTo(other.Date.Value);
        }

        public override bool Equals(object obj) => obj is ValidTo && Equals((ValidTo)obj);

        public bool Equals(ValidTo other) => Date == other.Date;

        public override int GetHashCode() => Date.GetHashCode();
    }
}
