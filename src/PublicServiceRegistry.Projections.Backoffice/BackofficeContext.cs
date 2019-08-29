namespace PublicServiceRegistry.Projections.Backoffice
{
    using System;
    using System.IO;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
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

    public class ConfigBasedContextFactory : IDesignTimeDbContextFactory<BackofficeContext>
    {
        public BackofficeContext CreateDbContext(string[] args)
        {
            const string migrationConnectionStringName = "BackofficeProjectionsAdmin";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(migrationConnectionStringName);
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Could not find a connection string with name '{migrationConnectionStringName}'");

            var builder = new DbContextOptionsBuilder<BackofficeContext>()
                .UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure();
                    sqlServerOptions.MigrationsHistoryTable(MigrationTables.Backoffice, Schema.Backoffice);
                })
                .UseExtendedSqlServerMigrations();

            return new BackofficeContext(builder.Options);
        }
    }
}
