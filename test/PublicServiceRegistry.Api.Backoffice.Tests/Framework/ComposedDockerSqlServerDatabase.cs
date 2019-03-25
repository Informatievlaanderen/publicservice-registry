namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// MsSql Server database provisioned via bitbucket pipelines on localhost:1433
    /// </summary>
    public class ComposedDockerSqlServerDatabase : ISqlServerDatabase
    {
        private readonly string _databaseName;
        private const string Password = "ContinuousIntegration1!";

        public ComposedDockerSqlServerDatabase(string databaseName)
            => _databaseName = databaseName;

        public SqlConnection CreateMasterConnection()
            => new SqlConnection(CreateMasterConnectionStringBuilder().ConnectionString);

        public SqlConnectionStringBuilder CreateMasterConnectionStringBuilder()
            => new SqlConnectionStringBuilder(
                    $"server=localhost,1433;User Id=sa;Password={Password};Initial Catalog=master");

        public SqlConnectionStringBuilder CreateConnectionStringBuilder()
            => new SqlConnectionStringBuilder(
                    $"server=localhost,1433;User Id=sa;Password={Password};Initial Catalog={_databaseName}");

        public async Task CreateDatabase(CancellationToken cancellationToken = default)
        {
            while (!await HealthCheck(cancellationToken).ConfigureAwait(false))
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);

            using (var connection = CreateMasterConnection())
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                using (var command = new SqlCommand($"CREATE DATABASE [{_databaseName}]", connection))
                    await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<bool> HealthCheck(CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = CreateMasterConnection())
                {
                    await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
