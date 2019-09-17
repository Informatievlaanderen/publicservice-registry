namespace PublicServiceRegistry.Projections.Backoffice
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using PublicServiceLabelList;
    using PublicServiceLifeCycle;
    using PublicServiceList;

    public class BackofficeContext : RunnerDbContext<BackofficeContext>
    {
        public const string DvrCodeSequenceName = "DvrCodes";

        public override string ProjectionStateSchema => Schema.Backoffice;

        public DbSet<PublicServiceListItem> PublicServiceList { get; set; }
        public DbSet<LifeCycleStageItemForPublicServiceList> LifeCycleStagesForPublicServiceList { get; set; }

        public DbSet<PublicServiceLabelListItem> PublicServiceLabelList { get; set; }

        public DbSet<PublicServiceLifeCycleItem> PublicServiceLifeCycleList { get; set; }

        // This needs to be here to please EF
        public BackofficeContext() { }

        // This needs to be DbContextOptions<T> for Autofac!
        public BackofficeContext(DbContextOptions<BackofficeContext> options)
            : base(options) { }
    }
}
