namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class RemovePublicServiceTests : AutofacBasedTest
    {
        public RemovePublicServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory]
        [DefaultData]
        public void CannotRemovePublicServiceOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new RemovePublicService(publicServiceId, reasonForRemoval).PerformedByAdmin())
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void RemovePublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new RemovePublicService(publicServiceId, reasonForRemoval).PerformedByAdmin())
                    .Then(publicServiceId,
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval)));
        }

        [Theory]
        [DefaultData]
        public void CantRemoveARemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                    .When(new RemovePublicService(publicServiceId, reasonForRemoval).PerformedByAdmin())
                    .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void NonAdminsCantRemoveAPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                    .When(new RemovePublicService(publicServiceId, reasonForRemoval))
                    .Throws(new InsufficientRights()));
        }

        [Theory]
        [DefaultData]
        public void BeheerdersCantRemoveAPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                    .When(new RemovePublicService(publicServiceId, reasonForRemoval).PerformedByBeheerder())
                    .Throws(new InsufficientRights()));
        }
    }
}
