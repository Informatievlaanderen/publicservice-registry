// ReSharper disable VirtualMemberCallInConstructor
namespace PublicServiceRegistry.PublicService
{
    using System;
    using Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Commands;
    using Pipes;
    using Providers;
    using SqlStreamStore;

    public class PublicServiceCommandHandlerModule : CommandHandlerModule
    {
        public PublicServiceCommandHandlerModule(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Func<IPublicServices> getPublicServices,
            IOrganisationProvider organisationProvider)
        {
            For<RegisterPublicService>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle((message, ct) =>
                {
                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = PublicService.Register(
                        publicServiceId,
                        message.Command.PublicServiceName,
                        message.Command.PrivateZoneId);

                    var publicServices = getPublicServices();
                    publicServices.Add(publicServiceId, publicService);

                    return Task.CompletedTask;
                });

            For<UpdatePublicService>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    var organisation = await organisationProvider.GetOrganisationAsync(message.Command.OvoNumber);

                    publicService.Rename(new PublicServiceName(message.Command.PublicServiceName));
                    publicService.AssignCompetentAuthority(organisation);
                    publicService.MarkForOrafinExport(message.Command.IsSubsidy);
                });

            For<UpdateLabels>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.UpdateLabels(message.Command.Labels);
                });

            For<AddStageToLifeCycle>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.AddStageToLifeCycle(
                        message.Command.LifeCycleStageType,
                        message.Command.LifeCycleStagePeriod);
                });

            For<ChangePeriodOfLifeCycleStage>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.ChangePeriodOfLifeCycleStage(
                        message.Command.LifeCycleStageId,
                        message.Command.LifeCycleStagePeriod);
                });

            For<RemoveStageFromLifeCycle>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.RemoveLifeCycleStage(
                        LifeCycleStageId.FromNumber(message.Command.LifeCycleStageId));
                });

            For<RemovePublicService>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .RequiresRole(PublicServiceRegistryClaims.AdminRole)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.Remove(message.Command.ReasonForRemoval);
                });

            For<SetIpdcCode>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .RequiresRole(PublicServiceRegistryClaims.AdminRole)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.SetIpdcCode(message.Command.IpdcCode);
                });

            For<SetLegislativeDocumentId>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .RequiresRole(PublicServiceRegistryClaims.AdminRole)
                .Handle(async (message, ct) =>
                {
                    var publicServices = getPublicServices();

                    var publicServiceId = message.Command.PublicServiceId;
                    var publicService = await publicServices.GetAsync(publicServiceId, ct);

                    publicService.SetLegislativeDocumentId(message.Command.LegislativeDocumentId);
                });
        }
    }
}
