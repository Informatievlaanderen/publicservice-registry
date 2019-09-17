namespace PublicServiceRegistry.Projections.Backoffice
{
    using System;
    using System.Data.SqlClient;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Sql.EntityFrameworkCore;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using Autofac;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class BackofficeModule : Module
    {
        public BackofficeModule(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<BackofficeModule>();
            var backofficeProjectionsConnectionString = configuration.GetConnectionString("BackofficeProjections");

            var hasConnectionString = !string.IsNullOrWhiteSpace(backofficeProjectionsConnectionString);
            if (hasConnectionString)
                RunOnSqlServer(configuration, services, loggerFactory, backofficeProjectionsConnectionString);
            else
                RunInMemoryDb(services, loggerFactory, logger);

            logger.LogInformation(
                "Added {Context} to services:" +
                Environment.NewLine +
                "\tSchema: {Schema}" +
                Environment.NewLine +
                "\tTableName: {TableName}",
                nameof(BackofficeContext), Schema.Backoffice, MigrationTables.Backoffice);
        }

        private static void RunOnSqlServer(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory,
            string backofficeProjectionsConnectionString)
        {
            services
                .AddScoped(s => new TraceDbConnection<BackofficeContext>(
                    new SqlConnection(backofficeProjectionsConnectionString),
                    configuration["DataDog:ServiceName"]))
                .AddDbContext<BackofficeContext>((provider, options) => options
                    .UseLoggerFactory(loggerFactory)
                    .UseSqlServer(provider.GetRequiredService<TraceDbConnection<BackofficeContext>>(), sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure();
                        sqlServerOptions.MigrationsHistoryTable(MigrationTables.Backoffice, Schema.Backoffice);
                    })
                    .UseExtendedSqlServerMigrations());
        }

        private static void RunInMemoryDb(IServiceCollection services, ILoggerFactory loggerFactory, ILogger<BackofficeModule> logger)
        {
            services
                .AddDbContext<BackofficeContext>(options => options
                    .UseLoggerFactory(loggerFactory)
                    .UseInMemoryDatabase(Guid.NewGuid().ToString(), sqlServerOptions => { }));

            logger.LogWarning("Running InMemory for {Context}!", nameof(BackofficeContext));
        }
    }
}
