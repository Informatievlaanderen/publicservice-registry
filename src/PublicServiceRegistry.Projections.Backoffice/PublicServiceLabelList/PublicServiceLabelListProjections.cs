namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLabelList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.EntityFrameworkCore;
    using PublicService.Events;

    public class PublicServiceLabelListProjections : ConnectedProjection<BackofficeContext>
    {
        public PublicServiceLabelListProjections()
        {
            When<Envelope<LabelWasAssigned>>(async (context, message, ct) =>
            {
                var publicServiceListItem = await FindPublicServiceLabelOrNull(
                    context,
                    message.Message.PublicServiceId,
                    message.Message.LabelType,
                    ct);

                if (publicServiceListItem == null)
                {
                    publicServiceListItem = new PublicServiceLabelListItem
                    {
                        PublicServiceId = message.Message.PublicServiceId,
                        LabelType = message.Message.LabelType,
                    };

                    await context
                        .PublicServiceLabelList
                        .AddAsync(publicServiceListItem, ct);
                }

                publicServiceListItem.LabelValue = message.Message.LabelValue;
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

        private static async Task<IEnumerable<PublicServiceLabelListItem>> AllItems(
            BackofficeContext context,
            string publicServiceId,
            CancellationToken cancellationToken)
        {
            var sqlEntities = await context
                .PublicServiceLabelList
                .Where(x => x.PublicServiceId == publicServiceId)
                .ToListAsync(cancellationToken);

            var localEntities = context
                .PublicServiceLabelList
                .Local
                .Where(x => x.PublicServiceId == publicServiceId)
                .ToList();

            return sqlEntities
                .Union(localEntities)
                .Distinct();
        }

        private static async Task<PublicServiceLabelListItem> FindPublicServiceLabelOrNull(
            BackofficeContext context,
            string publicServiceId,
            string labelType,
            CancellationToken cancellationToken)
            => await context
                .PublicServiceLabelList
                .FindAsync(
                    new object[]
                    {
                        publicServiceId,
                        labelType
                    },
                    cancellationToken);
    }
}
