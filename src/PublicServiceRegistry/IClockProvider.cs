namespace PublicServiceRegistry
{
    using System;
    using NodaTime;

    public interface IClockProvider
    {
        LocalDate Today { get; }
    }

    public class ClockProvider : IClockProvider
    {
        public LocalDate Today => LocalDate.FromDateTime(DateTime.Today);
    }
}
