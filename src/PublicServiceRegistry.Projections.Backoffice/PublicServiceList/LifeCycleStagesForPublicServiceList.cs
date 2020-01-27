namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;
    using PublicServiceLifeCycle;

    public class LifeCycleStageItemForPublicServiceList
    {
        public string PublicServiceId { get; set; }
        public int LifeCycleStageId { get; set; }
        public string? LifeCycleStageType { get; set; }

        public int? FromAsInt { get; set; }
        public int? ToAsInt { get; set; }

        public LocalDate? From
        {
            get => FromAsInt.ToLocalDate();
            set => FromAsInt = value.ToInt();
        }

        public LocalDate? To
        {
            get => ToAsInt.ToLocalDate();
            set => ToAsInt = value.ToInt();
        }
    }

    public class LifeCycleStagesForPublicServiceListConfiguration : IEntityTypeConfiguration<LifeCycleStageItemForPublicServiceList>
    {
        private const string TableName = "LifeCycleStagesForPublicServiceList";

        public void Configure(EntityTypeBuilder<LifeCycleStageItemForPublicServiceList> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, p.LifeCycleStageId })
                .IsClustered();

            b.Property(p => p.LifeCycleStageType);
            b.Property(p => p.FromAsInt).HasColumnName("From");
            b.Property(p => p.ToAsInt).HasColumnName("To");

            b.Ignore(p => p.From);
            b.Ignore(p => p.To);
        }
    }
}
