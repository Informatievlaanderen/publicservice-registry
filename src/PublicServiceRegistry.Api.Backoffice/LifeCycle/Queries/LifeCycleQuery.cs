namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceLifeCycle;
    using Swashbuckle.AspNetCore.Filters;

    public class LifeCycleQuery :
        Query<PublicServiceLifeCycleItem, LifeCycleFilter, PublicServiceLifeCycleResponseItem>
    {
        private readonly BackofficeContext _context;
        private readonly string _publicServiceId;

        protected override ISorting Sorting => new LifeCycleSorting();

        protected override Expression<Func<PublicServiceLifeCycleItem, PublicServiceLifeCycleResponseItem>> Transformation =>
            x => new PublicServiceLifeCycleResponseItem(
                x.PublicServiceId,
                x.LocalId,
                x.LifeCycleStageType,
                x.From,
                x.To);

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

    [DataContract(Name = "Levensloopfase", Namespace = "")]
    public class PublicServiceLifeCycleResponseItem
    {
        /// <summary>
        /// Id van de dienstverlening
        /// </summary>
        [DataMember(Name = "PublicServiceId", Order = 1)]
        public string PublicServiceId { get; }

        /// <summary>
        /// Locale id van de levensloopfase
        /// </summary>
        [DataMember(Name = "LocalId", Order = 5)]
        public int LocalId { get; }

        /// <summary>
        /// Id van het type van de levensloopfase
        /// </summary>
        [DataMember(Name = "LifeCycleStageTypeId", Order = 5)]
        public string LifeCycleStageTypeId { get; }

        /// <summary>
        /// Naam van het type van de levensloopfase
        /// </summary>
        [DataMember(Name = "LifeCycleStageTypeName", Order = 5)]
        public string LifeCycleStageTypeName { get; set; }

        /// <summary>
        /// Startdatum van de levensloopfase
        /// </summary>
        [DataMember(Name = "From", Order = 5)]
        public DateTime? From { get; }

        /// <summary>
        /// Einddatum van de levensloopfase
        /// </summary>
        [DataMember(Name = "To", Order = 5)]
        public DateTime? To { get; }


        public PublicServiceLifeCycleResponseItem(string publicServiceId, int localId, string lifeCycleStageTypeId, DateTime? from, DateTime? to)
        {
            PublicServiceId = publicServiceId;
            LocalId = localId;
            LifeCycleStageTypeId = lifeCycleStageTypeId;
            LifeCycleStageTypeName = PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageTypeId).Translation.Name;
            From = from;
            To = to;
        }
    }

    public class PublicServiceLifeCycleResponseItemExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<PublicServiceLifeCycleResponseItem>
            {
                new PublicServiceLifeCycleResponseItem(
                    "DVR000000001",
                    1,
                    "Active",
                    DateTime.Now,
                    DateTime.Now.AddDays(1)),

                new PublicServiceLifeCycleResponseItem(
                    "DVR000000001",
                    2,
                    "Planned",
                    DateTime.Now.AddDays(3),
                    DateTime.Now.AddDays(4)),
            };
        }
    }

    public class LifeCycleFilter { }
}
