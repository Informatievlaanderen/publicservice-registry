namespace PublicServiceRegistry.PublicService.Commands
{
    /// <summary>
    /// Temporary command to update the public service,
    /// made for Thomas his demo on 2018-04-23.
    /// </summary>
    public class UpdatePublicService
    {
        public PublicServiceId PublicServiceId { get; }
        public PublicServiceName PublicServiceName { get; }
        public OvoNumber OvoNumber { get; }
        public bool IsSubsidy { get; }

        public UpdatePublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            OvoNumber ovoNumber,
            bool isSubsidy)
        {
            PublicServiceId = publicServiceId;
            PublicServiceName = publicServiceName;
            OvoNumber = ovoNumber;
            IsSubsidy = isSubsidy;
        }
    }
}
