namespace PublicServiceRegistry.Api.Backoffice.Tests.Framework
{
    using Xunit;

    [CollectionDefinition(Name)]
    public class DockerTestCollection : ICollectionFixture<DockerFixture>
    {
        public const string Name = "DockerTests";
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
