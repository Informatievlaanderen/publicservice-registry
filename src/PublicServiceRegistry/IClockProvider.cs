namespace PublicServiceRegistry
{
    using System;
    using NodaTime;

    public interface IClockProvider
    {
        LocalDate Today { get; }
        DateTime Now { get; }
    }

    public class ClockProvider : IClockProvider
    {
        public LocalDate Today => LocalDate.FromDateTime(DateTime.Today);
        public DateTime Now => DateTime.Now;
    }
}
