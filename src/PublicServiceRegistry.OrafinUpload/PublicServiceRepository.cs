namespace PublicServiceRegistry.OrafinUpload
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceList;

    public interface IPublicServiceRepository
    {
        IEnumerable<PublicServiceListItem> GetActiveSubsidies();
    }

    public class PublicServiceRepository : IPublicServiceRepository
    {
        private readonly BackofficeContext _context;

        public PublicServiceRepository(BackofficeContext context) => _context = context;

        public IEnumerable<PublicServiceListItem> GetActiveSubsidies()
            => _context
                .PublicServiceList
                .AsNoTracking()
                .Where(x => !x.Removed)
                .Where(x => x.ExportToOrafin);
    }
}
