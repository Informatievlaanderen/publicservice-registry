namespace PublicServiceRegistry.Api.Backoffice.Tests
{
    using System;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Newtonsoft.Json;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceList;
    using PublicService;
    using PublicServiceRegistry.PublicService;
    using SqlStreamStore;
    using Xunit;
    using PublicService.Requests;
    using PublicServiceRegistry.PublicService.Providers;

    public class PublicServiceControllerTests
    {
        private readonly string _existingPublicServiceId;

        private readonly BackofficeContext _backofficeContext;
        private readonly EventMapping _eventMapping;
        private readonly EventDeserializer _eventDeserializer;

        public PublicServiceControllerTests()
        {
            var dbContextOptions = CreateDbContext();

            _existingPublicServiceId = "DVR000000001";

            var publicServiceListItem = new PublicServiceListItem
            {
                PublicServiceId = _existingPublicServiceId,
                Name = "Dienstverlening"
            };

            _backofficeContext = new BackofficeContext(dbContextOptions);
            _backofficeContext.PublicServiceList.Add(publicServiceListItem);
            _backofficeContext.SaveChanges();

            _eventMapping = new EventMapping(EventMapping.DiscoverEventNamesInAssembly(typeof(DomainAssemblyMarker).Assembly));
            _eventDeserializer = new EventDeserializer(JsonConvert.DeserializeObject);
        }

        private static DbContextOptions<BackofficeContext> CreateDbContext()
        {
            return new DbContextOptionsBuilder<BackofficeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        // [Fact]
        // public void WhenGettingANonExistingPublicService_Returns404()
        // {
        //     var streamStore = Mock.Of<IStreamStore>();
        //     var unitOfWork = new ConcurrentUnitOfWork();
        //     var sut = new PublicServiceController(new CommandHandlerResolver(new PublicServiceCommandHandlerModule(() => new PublicServices(unitOfWork,streamStore, _eventMapping, _eventDeserializer))));

        //     sut.Awaiting(controller => controller.Get(_backofficeContext, _nonExistingPublicServiceId, default))
        //         .ShouldThrow<ApiException>()
        //         .Which
        //         .StatusCode.Should().Be(StatusCodes.Status404NotFound);
        // }

        // [Fact]
        // public async Task WhenGettingAnExistingPublicService_Returns200WithPublicService()
        // {
        //     var streamStore = Mock.Of<IStreamStore>();
        //     var unitOfWork = new ConcurrentUnitOfWork();
        //     var sut = new PublicServiceController(new CommandHandlerResolver(new PublicServiceCommandHandlerModule(() => new PublicServices(unitOfWork, streamStore, _eventMapping, _eventDeserializer))));

        //     var actionResult = (ActionResult) await sut.Get(_backofficeContext, _existingPublicServiceId, default);

        //     actionResult.Should().BeOfType<OkObjectResult>()
        //         .Which.Value.As<PublicServiceResponse>()
        //         .ShouldBeEquivalentTo(new PublicServiceResponse(
        //             _existingPublicServiceId,
        //             "Dienstverlening")
        //         );
        // }

        //         [Fact]
        //         public async Task WhenListing_Returns200WithList()
        //         {
        //             var streamStore = Mock.Of<IStreamStore>();
        //             var unitOfWork = new ConcurrentUnitOfWork();
        //             var sut = new PublicServiceController(new CommandHandlerResolver(new PublicServiceCommandHandlerModule(() => new PublicServices(unitOfWork, streamStore, _eventMapping, _eventDeserializer))));
        //
        //             var actionResult = (ActionResult) await sut.List(default, _backofficeContext, null, string.Empty);
        //
        //             actionResult.Should().BeAssignableTo<OkObjectResult>()
        //                 .Which.Value.As<IEnumerable<PublicServiceResponse>>()
        //                 .ShouldBeEquivalentTo(
        //                     new List<PublicServiceResponse>
        //                     {
        //                         new PublicServiceResponse(
        //                             _existingPublicServiceId,
        //                             "Dienstverlening"
        //                         )
        //                     });
        //         }

        //[Fact]
        //public async Task WhenPosting_Returns200WithGuid()
        //{
        //    var streamStore = Mock.Of<IStreamStore>();
        //    var unitOfWork = new ConcurrentUnitOfWork();
        //    var organisationProviderMock = new Mock<IOrganisationProvider>();
        //    var sut = new PublicServiceController(new CommandHandlerResolver(new PublicServiceCommandHandlerModule(() => new PublicServices(unitOfWork, streamStore, _eventMapping, _eventDeserializer), organisationProviderMock.Object)));

        //    var actionResult = (ActionResult)await sut.Post(
        //        _backofficeContext,
        //        Guid.NewGuid(),
        //        new RegisterPublicServiceRequest { Naam = "Test" });

        //    actionResult.Should().BeOfType<AcceptedResult>()
        //        .Which.Value.Should().Be(-1L);
        //}
    }
}
