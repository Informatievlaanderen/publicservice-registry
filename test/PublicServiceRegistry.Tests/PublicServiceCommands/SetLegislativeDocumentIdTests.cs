namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class SetLegislativeDocumentIdTests : AutofacBasedTest
    {
        public SetLegislativeDocumentIdTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [DefaultData]
        public void NonAdminsCantSetLegislativeDocumentId(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LegislativeDocumentId legislativeDocumentId)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new SetLegislativeDocumentId(publicServiceId, legislativeDocumentId))
                    .Throws(new InsufficientRights()));
        }

        [Theory]
        [DefaultData]
        public void CantSetAnLegislativeDocumentIdOnARemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LegislativeDocumentId legislativeDocumentId)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                    .When(new SetLegislativeDocumentId(publicServiceId, legislativeDocumentId).PerformedByAdmin())
                    .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WhenLegislativeDocumentIdHasNotBeenSetBefore(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LegislativeDocumentId legislativeDocumentId)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new SetLegislativeDocumentId(publicServiceId, legislativeDocumentId).PerformedByAdmin())
                    .Then(publicServiceId,
                        new LegislativeDocumentIdWasSet(publicServiceId, legislativeDocumentId)));
        }

        [Theory]
        [DefaultData]
        public void WhenLegislativeDocumentIdHasBeenSetBefore(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LegislativeDocumentId previousLegislativeDocumentId,
            LegislativeDocumentId legislativeDocumentId)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new LegislativeDocumentIdWasSet(publicServiceId, previousLegislativeDocumentId))
                    .When(new SetLegislativeDocumentId(publicServiceId, legislativeDocumentId).PerformedByAdmin())
                    .Throws(new LegislativeDocumentIdWasAlreadySet()));
        }
    }
}
