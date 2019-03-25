namespace PublicServiceRegistry.Api.Backoffice.PublicService
{
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Projections.Backoffice;
    using PublicServiceRegistry.Infrastructure;
    using SqlStreamStore;
    using SqlStreamStore.Streams;

    public class DvrCodeGenerator
    {
        private readonly BackofficeContext _context;
        private readonly IStreamStore _streamStore;

        public DvrCodeGenerator(
            BackofficeContext context,
            IStreamStore streamStore)
        {
            _context = context;
            _streamStore = streamStore;
        }

        public async Task<PublicServiceId> GenerateDvrCode()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var publicServiceId = await GetNextPublicServiceId(connection);
                while (await StreamExists(publicServiceId))
                    publicServiceId = await GetNextPublicServiceId(connection);

                return publicServiceId;
            }
        }

        private static async Task<PublicServiceId> GetNextPublicServiceId(DbConnection connection)
        {
            var sqlStatement = $"SELECT NEXT VALUE FOR {Schema.Backoffice}.{BackofficeContext.DvrCodeSequenceName}";

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sqlStatement;

                var nextNumber = (int)await command.ExecuteScalarAsync();

                return PublicServiceId.FromNumber(nextNumber);
            }
        }

        private async Task<bool> StreamExists(PublicServiceId publicServiceId)
        {
            var pageReadStatus = await _streamStore
                .ReadStreamForwards(
                    streamId: publicServiceId.ToString(),
                    fromVersionInclusive: StreamVersion.Start,
                    maxCount: 1,
                    prefetchJsonData: false);

            return pageReadStatus.Status != PageReadStatus.StreamNotFound;
        }
    }
}
