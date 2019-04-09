namespace PublicServiceRegistry.Projections.Backoffice.PublicServiceList
{
    using System;
    using System.Globalization;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LifeCycleStageItemForPublicServiceList
    {
        private const string DateFormat = "yyyyMMdd";

        public string PublicServiceId { get; set; }
        public int LifeCycleStageId { get; set; }
        public string LifeCycleStageType { get; set; }

        public int? FromAsInt { get; set; }
        public int? ToAsInt { get; set; }

        public DateTime? From
        {
            get => FromAsInt.HasValue
                ? DateTime.ParseExact(FromAsInt.ToString(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)
                : (DateTime?) null;
            set => FromAsInt = value.HasValue
                ? Convert.ToInt32(value.Value.ToString(DateFormat, CultureInfo.InvariantCulture))
                : (int?) null;
        }

        public DateTime? To
        {
            get => ToAsInt.HasValue
                ? DateTime.ParseExact(ToAsInt.ToString(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)
                : (DateTime?)null;
            set => ToAsInt = value.HasValue
                ? Convert.ToInt32(value.Value.ToString(DateFormat, CultureInfo.InvariantCulture))
                : (int?)null;
        }
    }

    public class LifeCycleStagesForPublicServiceListConfiguration : IEntityTypeConfiguration<LifeCycleStageItemForPublicServiceList>
    {
        private const string TableName = "LifeCycleStagesForPublicServiceList";

        public void Configure(EntityTypeBuilder<LifeCycleStageItemForPublicServiceList> b)
        {
            b.ToTable(TableName, Schema.Backoffice)
                .HasKey(p => new { p.PublicServiceId, p.LifeCycleStageId })
                .ForSqlServerIsClustered();

            b.Property(p => p.LifeCycleStageType);
            b.Property(p => p.FromAsInt).HasColumnName("From");
            b.Property(p => p.ToAsInt).HasColumnName("To");

            b.Ignore(p => p.From);
            b.Ignore(p => p.To);
        }
    }
}
