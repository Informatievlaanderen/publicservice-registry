namespace PublicServiceRegistry
{
    using System;
    using System.Globalization;
    using NodaTime;

    public struct ValidFrom : IEquatable<ValidFrom>, IComparable<ValidFrom>
    {
        public LocalDate? Date { get; }

        public bool IsInfinite => !Date.HasValue;

        public ValidFrom(LocalDate? localDate) => Date = localDate;

        public ValidFrom(int year, int month, int day) => Date = new LocalDate(year, month, day);

        public static implicit operator LocalDate? (ValidFrom validFrom) => validFrom.Date;

        public override string ToString() => Date.HasValue ? Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "~";

        public bool IsInFutureOf(LocalDate date, bool inclusive = false) =>
            inclusive
                ? this >= new ValidFrom(date)
                : this > new ValidFrom(date);

        public bool IsInPastOf(LocalDate date, bool inclusive = false) =>
            inclusive
                ? this <= new ValidFrom(date)
                : this < new ValidFrom(date);

        public static bool operator ==(ValidFrom left, ValidFrom right) => left.Equals(right);

        public static bool operator !=(ValidFrom left, ValidFrom right) => !(left == right);

        public static bool operator <(ValidFrom left, ValidFrom right) => left.CompareTo(right) < 0;

        public static bool operator <=(ValidFrom left, ValidFrom right) => left.CompareTo(right) <= 0;

        public static bool operator >(ValidFrom left, ValidFrom right) => left.CompareTo(right) > 0;

        public static bool operator >=(ValidFrom left, ValidFrom right) => left.CompareTo(right) >= 0;

        public int CompareTo(ValidFrom other)
        {
            if (!Date.HasValue && !other.Date.HasValue)
                return 0;

            if (!Date.HasValue)
                return -1;

            if (!other.Date.HasValue)
                return 1;

            return Date.Value.CompareTo(other.Date.Value);
        }

        public override bool Equals(object obj) => obj is ValidFrom && Equals((ValidFrom)obj);

        public bool Equals(ValidFrom other) => Date == other.Date;

        public override int GetHashCode() => Date.GetHashCode();
    }
}
