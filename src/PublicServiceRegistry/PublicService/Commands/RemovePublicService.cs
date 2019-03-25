namespace PublicServiceRegistry.PublicService.Commands
{
    public class RemovePublicService
    {
        public PublicServiceId PublicServiceId { get; }
        public ReasonForRemoval ReasonForRemoval { get; }

        public RemovePublicService(
            PublicServiceId publicServiceId,
            ReasonForRemoval reasonForRemoval)
        {
            PublicServiceId = publicServiceId;
            ReasonForRemoval = reasonForRemoval;
        }
    }
}
