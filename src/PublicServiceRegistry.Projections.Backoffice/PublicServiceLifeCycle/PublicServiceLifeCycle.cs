namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;

    public class PublicServiceLifeCycleItem
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

    public class PublicServiceLifeCycleConfiguration : IEntityTypeConfiguration<PublicServiceLifeCycleItem>
    {
        private const string TableName = "PublicServiceLifeCycleList";

        public void Configure(EntityTypeBuilder<PublicServiceLifeCycleItem> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, LocalId = p.LifeCycleStageId })
                .IsClustered();

            b.Property(p => p.LifeCycleStageId).IsRequired();
            b.Property(p => p.LifeCycleStageType).IsRequired();
            b.Property(p => p.FromAsInt).HasColumnName("From");
            b.Property(p => p.ToAsInt).HasColumnName("To");

            b.Ignore(p => p.From);
            b.Ignore(p => p.To);
        }
    }
}
