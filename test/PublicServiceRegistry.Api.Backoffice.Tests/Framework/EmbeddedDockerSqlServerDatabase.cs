namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// MsSql Server database provisioned via docker on localhost:11433.
    /// </summary>
    public class EmbeddedDockerSqlServerDatabase : ISqlServerDatabase
    {
        private readonly string _databaseName;
        private readonly DockerContainer _sqlServerContainer;
        private const string Password =  "E@syP@ssw0rd";
        private const string Image = "microsoft/mssql-server-linux";
        private const string Tag = "2017-latest";
        private static readonly int Port = PortManager.GetNextPort();

        public EmbeddedDockerSqlServerDatabase(string databaseName)
        {
            _databaseName = databaseName;

            var ports = new Dictionary<int, int>
            {
                {1433, Port}
            };

            _sqlServerContainer = new DockerContainer(
                Image,
                Tag,
                HealthCheck,
                ports)
            {
                ContainerName = "publicserviceregistry-api-tests",
                Env = new[] {"ACCEPT_EULA=Y", $"SA_PASSWORD={Password}"}
            };
        }

        public SqlConnection CreateMasterConnection()
            => new SqlConnection(CreateMasterConnectionStringBuilder().ConnectionString);

        public SqlConnectionStringBuilder CreateMasterConnectionStringBuilder()
            => new SqlConnectionStringBuilder(
                    $"server=tcp:localhost,{Port};User Id=sa;Password={Password};Initial Catalog=master");

        public SqlConnectionStringBuilder CreateConnectionStringBuilder()
            => new SqlConnectionStringBuilder(
                    $"server=tcp:localhost,{Port};User Id=sa;Password={Password};Initial Catalog={_databaseName}");

        public async Task CreateDatabase(CancellationToken cancellationToken = default)
        {
            await _sqlServerContainer.TryStart(cancellationToken);

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
