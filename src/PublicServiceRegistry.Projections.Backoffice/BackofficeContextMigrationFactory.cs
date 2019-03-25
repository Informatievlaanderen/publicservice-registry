namespace PublicServiceRegistry.Projections.Backoffice
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class BackofficeContextMigrationFactory : RunnerDbContextMigrationFactory<BackofficeContext>
    {
        public BackofficeContextMigrationFactory()
            : base("BackofficeProjectionsAdmin", HistoryConfiguration)
        { }

        private static MigrationHistoryConfiguration HistoryConfiguration =>
            new MigrationHistoryConfiguration
            {
                Schema = Schema.Backoffice,
                Table = MigrationTables.Backoffice
            };

        protected override BackofficeContext CreateContext(DbContextOptions<BackofficeContext> migrationContextOptions)
            => new BackofficeContext(migrationContextOptions);
    }
}
