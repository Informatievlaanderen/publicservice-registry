namespace PublicServiceRegistry.PublicService.Commands
{
    public class SetLegislativeDocumentId
    {
        public PublicServiceId PublicServiceId { get; }
        public LegislativeDocumentId LegislativeDocumentId{ get; }

        public SetLegislativeDocumentId(
            PublicServiceId publicServiceId,
            LegislativeDocumentId legislativeDocumentId)
        {
            PublicServiceId = publicServiceId;
            LegislativeDocumentId = legislativeDocumentId;
        }
    }
}
