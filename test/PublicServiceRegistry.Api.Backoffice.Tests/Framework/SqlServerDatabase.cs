namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// MsSql Server database provisioned depending on the environment we are executing in (dev vs ci).
    /// </summary>
    public class SqlServerDatabase : ISqlServerDatabase
    {
        private readonly ISqlServerDatabase _database;

        public SqlServerDatabase(string databaseName)
        {
            if (Environment.GetEnvironmentVariable("CI") == null)
            {
                _database = new EmbeddedDockerSqlServerDatabase(databaseName);
            }
            else
            {
                _database = new ComposedDockerSqlServerDatabase(databaseName);
            }
        }

        public SqlConnection CreateMasterConnection()
            => _database.CreateMasterConnection();

        public SqlConnectionStringBuilder CreateMasterConnectionStringBuilder()
            => _database.CreateMasterConnectionStringBuilder();

        public SqlConnectionStringBuilder CreateConnectionStringBuilder()
            => _database.CreateConnectionStringBuilder();

        public Task CreateDatabase(CancellationToken cancellationToken = default)
            => _database.CreateDatabase(cancellationToken);
    }
}
