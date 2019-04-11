namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using AutoFixture;
    using NodaTime;

    public class WithLifeCycleStagePeriod : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<LifeCycleStagePeriod>(c => c.FromFactory(() =>
            {
                var from = fixture.Create<DateTime?>();
                var to = from.HasValue ?
                    from + fixture.Create<TimeSpan>() :
                    fixture.Create<DateTime?>();

                return new LifeCycleStagePeriod(new ValidFrom(from.HasValue ? LocalDate.FromDateTime(from.Value) : (LocalDate?) null), new ValidTo(to.HasValue ? LocalDate.FromDateTime(to.Value) : (LocalDate?) null));
            }));
        }
    }
}
