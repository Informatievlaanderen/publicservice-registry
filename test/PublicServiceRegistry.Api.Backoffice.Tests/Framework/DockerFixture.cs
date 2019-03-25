namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using System;

    public class DockerFixture : IDisposable
    {
        public Func<string, ISqlServerDatabase> GenerateDatabase { get; }

        public DockerFixture()
        {
            if (Environment.GetEnvironmentVariable("CI") == null)
            {
                GenerateDatabase = databaseName => new EmbeddedDockerSqlServerDatabase(databaseName);
            }
            else
            {
                GenerateDatabase = databaseName => new ComposedDockerSqlServerDatabase(databaseName);
            }
        }

        public void Dispose() { }
    }
}
