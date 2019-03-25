namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.ETag
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;

    public static class GetProjectionPositionExtension
    {
        public static Task<long> GetProjectionPositionAsync(this BackofficeContext context, CancellationToken cancellationToken)
            => context.GetProjectionPositionAsync(PublicServiceBackofficeRunner.Name, cancellationToken);

        public static async Task<long> GetProjectionPositionAsync(this BackofficeContext context, string projectionName, CancellationToken cancellationToken)
        {
            var projectionState =
                await context
                    .ProjectionStates
                    .AsNoTracking()
                    .SingleOrDefaultAsync(item => item.Name == projectionName, cancellationToken);

            return projectionState?.Position ?? -1L;
        }
    }
}
