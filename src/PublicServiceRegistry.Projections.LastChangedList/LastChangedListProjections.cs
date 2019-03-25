namespace PublicServiceRegistry.Projections.LastChangedList
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.LastChangedList;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using PublicService.Events;

    public class LastChangedListProjections : LastChangedListConnectedProjection
    {
        protected override string CacheKeyFormat => "legacy/publicservice:{{0}}.{1}";
        protected override string UriFormat => "/v1/dienstverleningen/{{0}}";

        private static readonly AcceptType[] SupportedAcceptTypes = { AcceptType.Json, AcceptType.Xml };

        public LastChangedListProjections()
            : base(SupportedAcceptTypes)
        {
            When<Envelope<PublicServiceWasRegistered>>(async (context, message, ct) =>
            {
                var attachedRecords = await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct);

                foreach (var record in attachedRecords)
                {
                    record.CacheKey = string.Format(record.CacheKey, message.Message.PublicServiceId);
                    record.Uri = string.Format(record.Uri, message.Message.PublicServiceId);
                }
            });

            When<Envelope<CompetentAuthorityWasAssigned>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<LabelWasAssigned>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<OrafinExportPropertyWasSet>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<PeriodOfLifeCycleStageWasChanged>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<PublicServiceWasRemoved>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<PublicServiceWasRenamed>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));

            When<Envelope<StageWasAddedToLifeCycle>>(async (context, message, ct)
                => await GetLastChangedRecordsAndUpdatePosition(message.Message.PublicServiceId.ToString(), message.Position, context, ct));
        }
    }
}
