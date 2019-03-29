namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicServiceLifeCycleItem
    {
        public string PublicServiceId { get; set; }
        public int LocalId { get; set; }
        public string LifeCycleStage { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class PublicServiceLifeCycleConfiguration : IEntityTypeConfiguration<PublicServiceLifeCycleItem>
    {
        private const string TableName = "PublicServiceLifeCycleList";

        public void Configure(EntityTypeBuilder<PublicServiceLifeCycleItem> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, p.LocalId })
                .ForSqlServerIsClustered();

            b.Property(p => p.LocalId).IsRequired();
            b.Property(p => p.LifeCycleStage).IsRequired();
            b.Property(p => p.From);
            b.Property(p => p.To);
        }
    }
}
