namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using AutoFixture;
    using Framework;
    using PublicService;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class SimpleUpdateTests : AutofacBasedTest
    {
        private readonly OrganisationMocker _mocker;

        public SimpleUpdateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _mocker = new OrganisationMocker(this);
        }

        [Theory]
        [DefaultData]
        public void CannotPerformSimpleUpdateOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PublicServiceName publicServiceRename,
            ReasonForRemoval reasonForRemoval,
            OvoNumber ovoNumber,
            Organisation organisation)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new UpdatePublicService(publicServiceId, publicServiceRename, null, false))
                .Throws(new CannotPerformActionOnRemovedPublicService()));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new UpdatePublicService(publicServiceId, publicServiceName, ovoNumber, false))
                .Throws(new CannotPerformActionOnRemovedPublicService()));

            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new UpdatePublicService(publicServiceId, publicServiceName, null, true))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WithOvoNumber(
            OvoNumber ovoNumber,
            Organisation organisation,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId))
                    .When(new UpdatePublicService(publicServiceId, publicServiceName, ovoNumber, false))
                    .Then(publicServiceId,
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            organisation.OvoNumber,
                            organisation.Name,
                            organisation.Provenance)));
        }

        [Theory]
        [DefaultData]
        public void WithEveryPropertyChanged(
            OvoNumber ovoNumber,
            Organisation organisation,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            var newPublicServiceName = new PublicServiceName("Uitreiken identiteitskaart");
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId))
                    .When(new UpdatePublicService(publicServiceId, newPublicServiceName, ovoNumber, true))
                    .Then(publicServiceId,
                        new PublicServiceWasRenamed(publicServiceId, newPublicServiceName),
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            organisation.OvoNumber,
                            organisation.Name,
                            organisation.Provenance),
                        new OrafinExportPropertyWasSet(
                            publicServiceId,
                            true)));
        }

        [Theory]
        [DefaultData]
        public void WithNameChanged(
            OvoNumber ovoNumber,
            Organisation organisation,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            var newPublicServiceName = new PublicServiceName("Uitreiken identiteitskaart");
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId),
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            organisation.OvoNumber,
                            organisation.Name,
                            organisation.Provenance))
                    .When(new UpdatePublicService(publicServiceId, newPublicServiceName, ovoNumber, false))
                    .Then(publicServiceId,
                        new PublicServiceWasRenamed(publicServiceId, newPublicServiceName)));
        }

        [Theory]
        [DefaultData]
        public void WithOvoNumberChanged(
            Fixture fixture,
            OvoNumber ovoNumber,
            Organisation organisation,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            var newOvoNumber = fixture.Create<OvoNumber>();
            var newOrganisation = fixture.Create<Organisation>();

            _mocker.MockOrganisationProvider(newOvoNumber, newOrganisation);

            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId),
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            organisation.OvoNumber,
                            organisation.Name,
                            organisation.Provenance))
                    .When(new UpdatePublicService(publicServiceId, publicServiceName, newOvoNumber, false))
                    .Then(publicServiceId,
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            newOrganisation.OvoNumber,
                            newOrganisation.Name,
                            newOrganisation.Provenance)));
        }

        [Theory]
        [DefaultData]
        public void WithOrafinExportChanged(
            OvoNumber ovoNumber,
            Organisation organisation,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProvider(ovoNumber, organisation);

            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId),
                        new CompetentAuthorityWasAssigned(
                            publicServiceId,
                            organisation.OvoNumber,
                            organisation.Name,
                            organisation.Provenance),
                        new OrafinExportPropertyWasSet(publicServiceId, true))
                    .When(new UpdatePublicService(publicServiceId, publicServiceName, ovoNumber, false))
                    .Then(publicServiceId,
                        new OrafinExportPropertyWasSet(publicServiceId, false)));
        }

        [Theory]
        [DefaultData]
        public void WithNonExistingOvoNumber(
            OvoNumber ovoNumber,
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            _mocker.MockOrganisationProviderFailure(ovoNumber, new GetOrganisationFailed());

            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, privateZoneId))
                    .When(new UpdatePublicService(publicServiceId, publicServiceName, ovoNumber, true))
                    .Throws(new GetOrganisationFailed())); // TODO: this only checks on type. Let's make the Throws method generic?
        }
    }
}
