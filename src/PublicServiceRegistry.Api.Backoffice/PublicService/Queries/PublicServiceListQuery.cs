namespace PublicServiceRegistry.Api.Backoffice.PublicService.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceList;
    using Responses;
    using Be.Vlaanderen.Basisregisters.Api.Search.Helpers;
    using Microsoft.EntityFrameworkCore;

    public class PublicServiceListQuery :
        Query<PublicServiceListItem, PublicServiceListItemFilter, PublicServiceListResponse>
    {
        private readonly BackofficeContext _context;

        protected override ISorting Sorting => new PublicServiceListSorting();

        protected override Expression<Func<PublicServiceListItem, PublicServiceListResponse>> Transformation =>
            x => new PublicServiceListResponse(
                x.PublicServiceId,
                x.Name,
                x.CompetentAuthorityCode,
                x.CompetentAuthorityName,
                x.ExportToOrafin);

        public PublicServiceListQuery(BackofficeContext context) => _context = context;

        protected override IQueryable<PublicServiceListItem> Filter(
            FilteringHeader<PublicServiceListItemFilter> filtering)
        {
            var publicServices = _context
                .PublicServiceList
                .AsNoTracking();

            if (!filtering.ShouldFilter)
                return publicServices;

            if (!filtering.Filter.Name.IsNullOrWhiteSpace())
                publicServices = publicServices.Where(x => x.Name.Contains(filtering.Filter.Name));

            if (!filtering.Filter.DvrCode.IsNullOrWhiteSpace())
                publicServices = publicServices.Where(x => x.PublicServiceId.Contains(filtering.Filter.DvrCode));

            if (!filtering.Filter.CompetentAuthority.IsNullOrWhiteSpace())
                publicServices = publicServices.Where(x => x.CompetentAuthorityName.Contains(filtering.Filter.CompetentAuthority));

            return publicServices;
        }

        private class PublicServiceListSorting : ISorting
        {
            public IEnumerable<string> SortableFields { get; } = new[]
            {
                nameof(PublicServiceListItem.PublicServiceId),
                nameof(PublicServiceListItem.Name),
                nameof(PublicServiceListItem.CompetentAuthorityName),
                nameof(PublicServiceListItem.ExportToOrafin)
            };

            public SortingHeader DefaultSortingHeader { get; } =
                new SortingHeader(nameof(PublicServiceListItem.PublicServiceId), SortOrder.Ascending);
        }
    }

    public class PublicServiceListItemFilter
    {
        public string Name { get; set; }
        public string DvrCode { get; set; }
        public string CompetentAuthority { get; set; }
    }
}
