namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class SetIpdcCodeTests : AutofacBasedTest
    {
        public SetIpdcCodeTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [DefaultData]
        public void NonAdminsCantSetIpdcCode(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            IpdcCode ipdcCode)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new SetIpdcCode(publicServiceId, ipdcCode))
                    .Throws(new InsufficientRights()));
        }

        [Theory]
        [DefaultData]
        public void CantSetAnIpdcCodeOnARemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            IpdcCode ipdcCode)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                    .When(new SetIpdcCode(publicServiceId, ipdcCode).PerformedByAdmin())
                    .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WhenIpdcCodeHasNotBeenSetBefore(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            IpdcCode ipdcCode)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new SetIpdcCode(publicServiceId, ipdcCode).PerformedByAdmin())
                    .Then(publicServiceId,
                        new IpdcCodeWasSet(publicServiceId, ipdcCode)));
        }

        [Theory]
        [DefaultData]
        public void WhenIpdcCodeHasBeenSetBefore(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            IpdcCode previousIpdcCode,
            IpdcCode ipdcCode)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new IpdcCodeWasSet(publicServiceId, previousIpdcCode))
                    .When(new SetIpdcCode(publicServiceId, ipdcCode).PerformedByAdmin())
                    .Throws(new IpdcCodeWasAlreadySet()));
        }
    }
}
