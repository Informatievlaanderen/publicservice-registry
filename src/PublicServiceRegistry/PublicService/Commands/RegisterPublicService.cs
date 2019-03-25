namespace PublicServiceRegistry.PublicService.Commands
{
    public class RegisterPublicService
    {
        public PublicServiceId PublicServiceId { get; }
        public PublicServiceName PublicServiceName { get; }
        public PrivateZoneId PrivateZoneId { get; }

        public RegisterPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            PrivateZoneId privateZoneId)
        {
            PublicServiceId = publicServiceId;
            PublicServiceName = publicServiceName;
            PrivateZoneId = privateZoneId;
        }
    }
}
