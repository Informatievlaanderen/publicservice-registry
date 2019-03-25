namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicServiceListItem
    {
        public string PublicServiceId { get; set; }
        public string Name { get; set; }
        public string CompetentAuthorityCode { get; set; }
        public string CompetentAuthorityName { get; set; }
        public bool ExportToOrafin { get; set; }
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

            b.HasIndex(p => p.Name);
        }
    }
}
