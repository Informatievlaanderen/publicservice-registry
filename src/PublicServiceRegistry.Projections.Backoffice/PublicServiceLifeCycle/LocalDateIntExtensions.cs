namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle
{
    using System;
    using System.Globalization;
    using NodaTime;
    using PublicServiceList;

    public static class LocalDateIntExtensions
    {
        public static LocalDate? ToLocalDate(this int? source)
        {
            return source.HasValue
                ? LocalDate.FromDateTime(DateTime.ParseExact(source.ToString(), LocalDateFormat.Format,
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal))
                : (LocalDate?) null;
        }

        public static int? ToInt(this LocalDate? source)
        {
            return source?.ToInt();
        }

        public static int? ToInt(this LocalDate source)
        {
            return Convert.ToInt32(source.ToString(LocalDateFormat.Format, CultureInfo.InvariantCulture));
        }
    }
}
