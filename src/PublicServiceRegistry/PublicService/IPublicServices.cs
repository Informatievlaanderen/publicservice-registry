namespace PublicServiceRegistry.PublicService
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IPublicServices : IAsyncRepository<PublicService, PublicServiceId> { }
}
