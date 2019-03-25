namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISqlServerDatabase
    {
        SqlConnection CreateMasterConnection();

        SqlConnectionStringBuilder CreateMasterConnectionStringBuilder();

        SqlConnectionStringBuilder CreateConnectionStringBuilder();

        Task CreateDatabase(CancellationToken cancellationToken = default);
    }
}
