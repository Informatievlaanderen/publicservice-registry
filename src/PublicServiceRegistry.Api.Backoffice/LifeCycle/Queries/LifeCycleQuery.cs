namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceLifeCycle;

    public class LifeCycleQuery :
        Query<PublicServiceLifeCycleItem, LifeCycleFilter, PublicServiceLifeCycleItem>
    {
        private readonly BackofficeContext _context;
        private readonly string _publicServiceId;

        protected override ISorting Sorting => new LifeCycleSorting();

        protected override Expression<Func<PublicServiceLifeCycleItem, PublicServiceLifeCycleItem>> Transformation =>
            x => x;

        public LifeCycleQuery(BackofficeContext context, string publicServiceId)
        {
            _context = context;
            _publicServiceId = publicServiceId;
        }

        protected override IQueryable<PublicServiceLifeCycleItem> Filter(
            FilteringHeader<LifeCycleFilter> filtering) =>
            _context
                .PublicServiceLifeCycleList
                .AsNoTracking()
                .Where(item => item.PublicServiceId == _publicServiceId);

        private class LifeCycleSorting : ISorting
        {
            public IEnumerable<string> SortableFields { get; } = new[]
            {
                nameof(PublicServiceLifeCycleItem.LifeCycleStageType),
                nameof(PublicServiceLifeCycleItem.From),
                nameof(PublicServiceLifeCycleItem.To)
            };

            public SortingHeader DefaultSortingHeader { get; } =
                new SortingHeader(nameof(PublicServiceLifeCycleItem.From), SortOrder.Ascending);
        }
    }

    public class LifeCycleFilter { }
}
