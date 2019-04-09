namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;

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

        public DateTime Today => _now.Date;
    }
}