namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using PublicServiceRegistry.PublicService.Events;

    public class PublicServiceListProjections : ConnectedProjection<BackofficeContext>
    {
        public PublicServiceListProjections()
        {
            When<Envelope<PublicServiceWasRegistered>>(async (context, message, ct) =>
            {
                await context
                    .PublicServiceList
                    .AddAsync(
                        new PublicServiceListItem
                        {
                            PublicServiceId = message.Message.PublicServiceId,
                            Name = message.Message.Name
                        },
                        ct);
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
    }
}
