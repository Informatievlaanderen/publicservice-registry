namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.ETag
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;

    public static class GetProjectionPositionExtension
    {
        public static async Task<long> GetProjectionPositionAsync(this BackofficeContext context, Type projection, CancellationToken cancellationToken)
        {
            var projectionName = projection.FullName;

            var projectionState =
                await context
                    .ProjectionStates
                    .AsNoTracking()
                    .SingleOrDefaultAsync(item => item.Name == projectionName, cancellationToken);

            return projectionState?.Position ?? -1L;
        }
    }
}
