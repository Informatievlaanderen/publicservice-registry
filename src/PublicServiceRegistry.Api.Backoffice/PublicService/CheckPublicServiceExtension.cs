namespace PublicServiceRegistry.Api.Backoffice.PublicService
{
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceList;

    public static class CheckPublicServiceExtension
    {
        public static async Task CheckPublicServiceAsync(
            this BackofficeContext context,
            string id,
            CancellationToken cancellationToken)
        {
            var publicService =
                await context
                    .PublicServiceList
                    .AsNoTracking()
                    .SingleOrDefaultAsync(item => item.PublicServiceId == id, cancellationToken);

            publicService.CheckPublicService();
        }

        public static void CheckPublicService(
            this PublicServiceListItem publicService)
        {
            if (publicService == null)
                throw new ApiException("Onbestaande dienstverlening.", StatusCodes.Status404NotFound);

            if (publicService.Removed)
                throw new ApiException("Dienstverlening werd verwijderd.", StatusCodes.Status410Gone);
        }
    }
}
