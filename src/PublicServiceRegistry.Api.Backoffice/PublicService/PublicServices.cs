namespace PublicServiceRegistry.Api.Backoffice.PublicService
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using PublicServiceRegistry.PublicService;
    using SqlStreamStore;

    public class PublicServices : Repository<PublicService, PublicServiceId>, IPublicServices
    {
        public PublicServices(ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer)
            : base(PublicService.Factory, unitOfWork, eventStore, eventMapping, eventDeserializer) { }
    }
}
