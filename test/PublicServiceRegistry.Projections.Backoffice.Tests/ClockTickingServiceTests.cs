namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Projector.Infrastructure.HostedServices;
    using PublicServiceRegistry.PublicService.Events;
    using SqlStreamStore;
    using SqlStreamStore.Streams;
    using Xunit;

    public class ClockTickingServiceTests
    {
        private readonly EventMapping _eventMapping;

        public ClockTickingServiceTests()
        {
            _eventMapping = new EventMapping(EventMapping.DiscoverEventNamesInAssembly(typeof(ClockHasTicked).Assembly));
        }

        [Fact]
        public async Task AppendsEventWhenStreamIsEmpty()
        {
            var now = new DateTime(2019, 12, 25, 12, 00, 00);
            var utcNow = now.ToUniversalTime();
            var clockProviderStub = new ClockProviderStub(now);
            var inMemoryStreamStore = new InMemoryStreamStore(() => utcNow);

            var sut = new ClockTickingService(inMemoryStreamStore, clockProviderStub);

            var cancellationToken = CancellationToken.None;
            await sut.StartAsync(cancellationToken);

            var forwards = await inMemoryStreamStore.ReadAllForwards(Position.Start, Int32.MaxValue, true, cancellationToken);

            forwards.Messages.Should().HaveCount(1);

            var message = forwards.Messages[0];
            message.StreamId.Should().Be(ClockTickingService.ClockStreamId);
            message.StreamVersion.Should().Be(StreamVersion.Start);

            var messageData = JsonConvert.DeserializeObject<ClockHasTicked>(await message.GetJsonData(cancellationToken));
            messageData.DateTime.Should().Be(now);
        }

        [Fact]
        public async Task AppendsEventWhenLastEventIsFromDifferentDay()
        {
            var now = new DateTime(2019, 12, 25, 12, 00, 00);
            var utcNow = now.ToUniversalTime();
            var clockProviderStub = new ClockProviderStub(now);
            var inMemoryStreamStore = new InMemoryStreamStore(() => utcNow);
            var clockHasTicked = new ClockHasTicked(new DateTime(2019, 12, 24, 12, 00, 00));

            var cancellationToken = CancellationToken.None;
            await inMemoryStreamStore.AppendToStream(
                streamId: new StreamId(ClockTickingService.ClockStreamId),
                expectedVersion: ExpectedVersion.Any,
                message: new NewStreamMessage(
                    messageId: Guid.NewGuid(),
                    type: _eventMapping.GetEventName(clockHasTicked.GetType()),
                    jsonData: JsonConvert.SerializeObject(clockHasTicked)),
                cancellationToken: cancellationToken);

            var sut = new ClockTickingService(inMemoryStreamStore, clockProviderStub);

            await sut.StartAsync(cancellationToken);

            var forwards = await inMemoryStreamStore.ReadAllForwards(Position.Start, Int32.MaxValue, true, cancellationToken);

            forwards.Messages.Should().HaveCount(2);

            var message = forwards.Messages[1];
            message.StreamId.Should().Be(ClockTickingService.ClockStreamId);
            message.StreamVersion.Should().Be(1);

            var messageData = JsonConvert.DeserializeObject<ClockHasTicked>(await message.GetJsonData(cancellationToken));
            messageData.DateTime.Should().Be(now);
        }

        [Fact]
        public async Task DoesNotAppendEventWhenLastEventIsFromSameDay()
        {
            var now = new DateTime(2019, 12, 25, 12, 00, 00);
            var utcNow = now.ToUniversalTime();
            var clockProviderStub = new ClockProviderStub(now);
            var inMemoryStreamStore = new InMemoryStreamStore(() => utcNow);
            var clockHasTicked = new ClockHasTicked(new DateTime(2019, 12, 25, 11, 00, 00));

            var cancellationToken = CancellationToken.None;
            await inMemoryStreamStore.AppendToStream(
                streamId: new StreamId(ClockTickingService.ClockStreamId),
                expectedVersion: ExpectedVersion.Any,
                message: new NewStreamMessage(
                    messageId: Guid.NewGuid(),
                    type: _eventMapping.GetEventName(clockHasTicked.GetType()),
                    jsonData: JsonConvert.SerializeObject(clockHasTicked)),
                cancellationToken: cancellationToken);

            var sut = new ClockTickingService(inMemoryStreamStore, clockProviderStub);

            await sut.StartAsync(cancellationToken);

            var forwards = await inMemoryStreamStore.ReadAllForwards(Position.Start, Int32.MaxValue, true, cancellationToken);

            forwards.Messages.Should().HaveCount(1);
        }
    }
}
