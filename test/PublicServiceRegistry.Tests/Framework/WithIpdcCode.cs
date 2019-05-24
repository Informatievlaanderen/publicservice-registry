namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using AutoFixture;

    public class WithIpdcCode : ICustomization
    {
        public void Customize(IFixture fixture) =>
            fixture.Customize<IpdcCode>(c => c.FromFactory(() => new IpdcCode(new Random().Next(1,10000).ToString("D4"))));
    }
}
