namespace PublicServiceRegistry.Projector.Infrastructure.HostedServices
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;
    using NodaTime;
    using PublicService.Events;
    using SqlStreamStore;
    using SqlStreamStore.Streams;

    public class ClockTickingService : HostedService
    {
        public const string ClockStreamId = "Clock";

        private readonly IStreamStore _streamStore;
        private readonly IClockProvider _clockProvider;

        public ClockTickingService(IStreamStore streamStore, IClockProvider clockProvider)
        {
            _streamStore = streamStore;
            _clockProvider = clockProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Tick(cancellationToken);
                await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
            }
        }

        private async Task Tick(CancellationToken cancellationToken)
        {
            var eventMapping = new EventMapping(EventMapping.DiscoverEventNamesInAssembly(typeof(ClockHasTicked).Assembly));

            var clockStreamId = new StreamId(ClockStreamId);
            var lastClockHasTicked = await _streamStore.ReadStreamBackwards(
                streamId: clockStreamId,
                fromVersionInclusive: StreamVersion.End,
                maxCount: 1,
                prefetchJsonData: true,
                cancellationToken: cancellationToken);

            if (!lastClockHasTicked.Messages.Any())
            {
                await AppendClockHasTicked(cancellationToken, clockStreamId, eventMapping);
            }
            else
            {
                var lastClockTickJsonData = await lastClockHasTicked.Messages[0].GetJsonData(cancellationToken);
                var lastClockTick = JsonConvert.DeserializeObject<ClockHasTicked>(lastClockTickJsonData);

                if (LocalDate.FromDateTime(lastClockTick.DateTime.Date) != _clockProvider.Today)
                {
                    await AppendClockHasTicked(cancellationToken, clockStreamId, eventMapping);
                }
            }
        }

        private async Task AppendClockHasTicked(
            CancellationToken cancellationToken,
            StreamId clockStreamId,
            EventMapping eventMapping)
        {
            var clockHasTicked = new ClockHasTicked(_clockProvider.Now);

            await _streamStore.AppendToStream(
                streamId: clockStreamId,
                expectedVersion: ExpectedVersion.Any,
                message: new NewStreamMessage(
                    messageId: Guid.NewGuid(),
                    type: eventMapping.GetEventName(clockHasTicked.GetType()),
                    jsonData: JsonConvert.SerializeObject(clockHasTicked)),
                cancellationToken: cancellationToken);
        }
    }
}
