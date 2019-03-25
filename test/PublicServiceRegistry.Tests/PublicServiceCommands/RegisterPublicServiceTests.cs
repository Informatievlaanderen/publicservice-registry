namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using Xunit;
    using Xunit.Abstractions;

    public class RegisterPublicServiceTests : AutofacBasedTest
    {
        public RegisterPublicServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory]
        [DefaultData]
        public void RegisterWithIdAndName(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            Assert(
                new Scenario()
                    .Given()
                    .When(new RegisterPublicService(publicServiceId, publicServiceName, privateZoneId))
                    .Then(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId)));
        }
    }
}
