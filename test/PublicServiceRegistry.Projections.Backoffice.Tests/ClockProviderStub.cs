namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using NodaTime;

    public class ClockProviderStub : IClockProvider
    {
        private DateTime _now;

        public ClockProviderStub(in DateTime now)
        {
            _now = now;
        }

        public void ChangeDate(in DateTime now)
        {
            _now = now;
        }

        public LocalDate Today => LocalDate.FromDateTime(_now.Date);
        public DateTime Now => _now;
    }
}
