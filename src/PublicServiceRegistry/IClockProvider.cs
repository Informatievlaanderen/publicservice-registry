namespace PublicServiceRegistry
{
    using System;

    public interface IClockProvider
    {
        DateTime Today { get; }
    }

    public class ClockProvider : IClockProvider
    {
        public DateTime Today => DateTime.Today;
    }
}
