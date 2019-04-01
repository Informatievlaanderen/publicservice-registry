namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.EntityFrameworkCore;
    using PublicServiceRegistry.PublicService.Events;

    public class PublicServiceLifeCycleListProjections : ConnectedProjection<BackofficeContext>
    {
        public PublicServiceLifeCycleListProjections()
        {
            When<Envelope<StageWasAddedToLifeCycle>>(async (context, message, ct) =>
            {
                var publicServiceLifeCycleItem = new PublicServiceLifeCycleItem
                {
                    PublicServiceId = message.Message.PublicServiceId,
                    LocalId = message.Message.Id,
                    LifeCycleStageType = message.Message.LifeCycleStageType,
                    From = message.Message.From,
                    To = message.Message.To
                };

                await context
                    .PublicServiceLifeCycleList
                    .AddAsync(publicServiceLifeCycleItem, ct);
            });

            When<Envelope<PeriodOfLifeCycleStageWasChanged>>(async (context, message, ct) =>
            {
                var publicServiceLifeCycleItem = await FindPublicServiceLifeCycleItemOrNull(
                    context,
                    message.Message.PublicServiceId,
                    message.Message.Id,
                    ct);

                publicServiceLifeCycleItem.From = message.Message.From;
                publicServiceLifeCycleItem.To = message.Message.To;
            });

            When<Envelope<PublicServiceWasRemoved>>(async (context, message, ct) =>
            {
                var items = await AllItems(
                    context,
                    message.Message.PublicServiceId,
                    ct);

                // TODO: Double check if this works in catch-up mode
                foreach (var item in items)
                    context.Remove(item);
            });
        }

        private static async Task<IEnumerable<PublicServiceLifeCycleItem>> AllItems(
            BackofficeContext context,
            string publicServiceId,
            CancellationToken cancellationToken)
        {
            var sqlEntities = await context
                .PublicServiceLifeCycleList
                .Where(x => x.PublicServiceId == publicServiceId)
                .ToListAsync(cancellationToken);

            var localEntities = context
                .PublicServiceLifeCycleList
                .Local
                .Where(x => x.PublicServiceId == publicServiceId)
                .ToList();

            return sqlEntities
                .Union(localEntities)
                .Distinct();
        }

        private static async Task<PublicServiceLifeCycleItem> FindPublicServiceLifeCycleItemOrNull(
            BackofficeContext context,
            string publicServiceId,
            int localId,
            CancellationToken cancellationToken)
            => await context
                .PublicServiceLifeCycleList
                .FindAsync(
                    new object[]
                    {
                        publicServiceId,
                        localId
                    }, cancellationToken);
    }
}
