namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLabelList
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicServiceLabelListItem
    {
        public string PublicServiceId { get; set; }
        public string LabelType { get; set; }
        public string? LabelValue { get; set; }
    }

    public class PublicServiceLabelListConfiguration : IEntityTypeConfiguration<PublicServiceLabelListItem>
    {
        private const string TableName = "PublicServiceLabelList";

        public void Configure(EntityTypeBuilder<PublicServiceLabelListItem> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, p.LabelType })
                .IsClustered();

            b.Property(p => p.LabelType);
            b.Property(p => p.LabelValue);
        }
    }
}
