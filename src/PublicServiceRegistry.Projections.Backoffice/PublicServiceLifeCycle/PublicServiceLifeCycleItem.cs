namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicServiceLifeCycleItem
    {
        public string PublicServiceId { get; set; }
        public int LifeCycleStageId { get; set; }
        public string LifeCycleStageType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class PublicServiceLifeCycleConfiguration : IEntityTypeConfiguration<PublicServiceLifeCycleItem>
    {
        private const string TableName = "PublicServiceLifeCycleList";

        public void Configure(EntityTypeBuilder<PublicServiceLifeCycleItem> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, LocalId = p.LifeCycleStageId })
                .ForSqlServerIsClustered();

            b.Property(p => p.LifeCycleStageId).IsRequired();
            b.Property(p => p.LifeCycleStageType).IsRequired();
            b.Property(p => p.From);
            b.Property(p => p.To);
        }
    }
}
