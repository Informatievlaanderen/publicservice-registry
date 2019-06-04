namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using PublicServiceLifeCycle;
    using PublicServiceRegistry.PublicService.Events;

    public class PublicServiceListProjections : ConnectedProjection<BackofficeContext>
    {
        public PublicServiceListProjections(IClockProvider clockProvider)
        {
            When<Envelope<PublicServiceWasRegistered>>(async (context, message, ct) =>
            {
                var publicServiceListItem = new PublicServiceListItem
                {
                    PublicServiceId = message.Message.PublicServiceId,
                    Name = message.Message.Name
                };

                await context
                    .PublicServiceList
                    .AddAsync(publicServiceListItem, ct);
            });

            When<Envelope<PublicServiceWasRenamed>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.Name = message.Message.NewName;
            });

            When<Envelope<CompetentAuthorityWasAssigned>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.CompetentAuthorityCode = message.Message.CompetentAuthorityCode;
                publicServiceListItem.CompetentAuthorityName = message.Message.CompetentAuthorityName;
            });

            When<Envelope<OrafinExportPropertyWasSet>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.ExportToOrafin = message.Message.ExportToOrafin;
            });

            When<Envelope<PublicServiceWasRemoved>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                context.Remove(publicServiceListItem);
            });

            When<Envelope<StageWasAddedToLifeCycle>>(async (context, message, ct) =>
            {
                await AddLifeCycleStage(message, context, ct);

                var period = new LifeCycleStagePeriod(
                    new ValidFrom(message.Message.From),
                    new ValidTo(message.Message.To));

                if (!period.OverlapsWith(clockProvider.Today))
                    return;

                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.CurrentLifeCycleStageType = message.Message.LifeCycleStageType;
                publicServiceListItem.CurrentLifeCycleStageId = message.Message.LifeCycleStageId;
                publicServiceListItem.CurrentLifeCycleStageEndsAt = message.Message.To;
            });

            When<Envelope<PeriodOfLifeCycleStageWasChanged>>(async (context, message, ct) =>
            {
                var publicServiceLifeCycleItem = await FindPublicServiceLifeCycleItemOrNull(
                    context,
                    message.Message.PublicServiceId,
                    message.Message.LifeCycleStageId,
                    ct);

                publicServiceLifeCycleItem.From = message.Message.From;
                publicServiceLifeCycleItem.To = message.Message.To;

                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                var period = new LifeCycleStagePeriod(new ValidFrom(message.Message.From), new ValidTo(message.Message.To));
                if (period.OverlapsWith(clockProvider.Today))
                {
                    publicServiceListItem.CurrentLifeCycleStageId = message.Message.LifeCycleStageId;
                    publicServiceListItem.CurrentLifeCycleStageType = publicServiceLifeCycleItem.LifeCycleStageType;
                    publicServiceListItem.CurrentLifeCycleStageEndsAt = message.Message.To;
                }
                else if (publicServiceListItem.CurrentLifeCycleStageId == message.Message.LifeCycleStageId &&
                         !period.OverlapsWith(clockProvider.Today))
                {
                    publicServiceListItem.CurrentLifeCycleStageId = null;
                    publicServiceListItem.CurrentLifeCycleStageType = null;
                    publicServiceListItem.CurrentLifeCycleStageEndsAt = null;
                }
            });

            When<Envelope<LifeCycleStageWasRemoved>>(async (context, message, ct) =>
            {
                var publicServiceLifeCycleItem = await FindPublicServiceLifeCycleItemOrNull(
                    context,
                    message.Message.PublicServiceId,
                    message.Message.LifeCycleStageId,
                    ct);

                context.LifeCycleStagesForPublicServiceList.Remove(publicServiceLifeCycleItem);

                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                if (publicServiceListItem.CurrentLifeCycleStageId == publicServiceLifeCycleItem.LifeCycleStageId)
                {
                    publicServiceListItem.CurrentLifeCycleStageId = null;
                    publicServiceListItem.CurrentLifeCycleStageType = null;
                    publicServiceListItem.CurrentLifeCycleStageEndsAt = null;
                }
            });

            When<Envelope<IpdcCodeWasSet>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.IpdcCode = message.Message.IpdcCode;
            });

            When<Envelope<LegislativeDocumentIdWasSet>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicService(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                publicServiceListItem.LegislativeDocumentId = message.Message.LegislativeDocumentId;
            });

            When<Envelope<ClockHasTicked>>(async (context, message, ct) =>
            {
                var date = LocalDate.FromDateTime(message.Message.DateTime);
                var dateAsInt = date.ToInt();
                foreach (var publicServiceListItem in context.PublicServiceList.Where(item => item.CurrentLifeCycleStageEndsAtAsInt != null && item.CurrentLifeCycleStageEndsAtAsInt < dateAsInt))
                {
                    await UpdateCurrentLifeCycleStage(context, publicServiceListItem, date, ct);
                }
            });
        }

        private static async Task AddLifeCycleStage(Envelope<StageWasAddedToLifeCycle> message, BackofficeContext context, CancellationToken ct)
        {
            var publicServiceLifeCycleItem = new LifeCycleStageItemForPublicServiceList
            {
                PublicServiceId = message.Message.PublicServiceId,
                LifeCycleStageId = message.Message.LifeCycleStageId,
                LifeCycleStageType = message.Message.LifeCycleStageType,
                From = message.Message.From,
                To = message.Message.To
            };

            await context
                .LifeCycleStagesForPublicServiceList
                .AddAsync(publicServiceLifeCycleItem, ct);
        }

        private static async Task UpdateCurrentLifeCycleStage(
            BackofficeContext context,
            PublicServiceListItem publicServiceListItem,
            LocalDate today,
            CancellationToken cancellationToken)
        {
            var todayAsInt = today.ToInt();
            var currentLifeCycleStage = await
                context
                    .LifeCycleStagesForPublicServiceList
                    .SingleOrDefaultAsync(stage =>
                        stage.PublicServiceId == publicServiceListItem.PublicServiceId &&
                        (stage.FromAsInt == null || stage.FromAsInt <= todayAsInt) &&
                        (stage.ToAsInt == null || stage.ToAsInt >= todayAsInt), cancellationToken);

            publicServiceListItem.CurrentLifeCycleStageType = currentLifeCycleStage?.LifeCycleStageType;
            publicServiceListItem.CurrentLifeCycleStageId = currentLifeCycleStage?.LifeCycleStageId;
            publicServiceListItem.CurrentLifeCycleStageEndsAt = currentLifeCycleStage?.To;
        }

        private static async Task<PublicServiceListItem> FindPublicService(
            BackofficeContext context,
            string publicServiceId,
            CancellationToken cancellationToken)
            => await context
                .PublicServiceList
                .FindAsync(
                    new object[] { publicServiceId },
                    cancellationToken);


        private static async Task<LifeCycleStageItemForPublicServiceList> FindPublicServiceLifeCycleItemOrNull(
            BackofficeContext context,
            string publicServiceId,
            int localId,
            CancellationToken cancellationToken)
            => await context.LifeCycleStagesForPublicServiceList
                .FindAsync(
                    new object[]
                    {
                        publicServiceId,
                        localId
                    }, cancellationToken);
    }
}
