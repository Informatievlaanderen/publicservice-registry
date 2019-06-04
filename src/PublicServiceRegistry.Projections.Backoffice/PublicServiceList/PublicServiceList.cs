namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;
    using PublicServiceLifeCycle;

    public class PublicServiceListItem
    {
        public string PublicServiceId { get; set; }
        public string Name { get; set; }
        public string CompetentAuthorityCode { get; set; }
        public string CompetentAuthorityName { get; set; }
        public bool ExportToOrafin { get; set; }
        public string CurrentLifeCycleStageType { get; set; }
        public int? CurrentLifeCycleStageId { get; set; }
        public int? CurrentLifeCycleStageEndsAtAsInt { get; set; }
        public string IpdcCode { get; set; }


        public LocalDate? CurrentLifeCycleStageEndsAt
        {
            get => CurrentLifeCycleStageEndsAtAsInt.ToLocalDate();
            set => CurrentLifeCycleStageEndsAtAsInt = value.ToInt();
        }
    }

    public class PublicServiceListConfiguration : IEntityTypeConfiguration<PublicServiceListItem>
    {
        private const string TableName = "PublicServiceList";

        public void Configure(EntityTypeBuilder<PublicServiceListItem> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => p.PublicServiceId)
                .ForSqlServerIsClustered();

            b.Property(p => p.Name);
            b.Property(p => p.CompetentAuthorityCode);
            b.Property(p => p.CompetentAuthorityName);
            b.Property(p => p.ExportToOrafin);
            b.Property(p => p.CurrentLifeCycleStageId);
            b.Property(p => p.CurrentLifeCycleStageType);
            b.Property(p => p.CurrentLifeCycleStageEndsAtAsInt).HasColumnName("CurrentLifeCycleStageEndsAt");
            b.Property(p => p.IpdcCode);

            b.Ignore(p => p.CurrentLifeCycleStageEndsAt);

            b.HasIndex(p => p.Name);
        }
    }
}
