namespace PublicServiceRegistry.PublicService.Commands
{
    public class SetIpdcCode
    {
        public PublicServiceId PublicServiceId { get; }
        public IpdcCode IpdcCode{ get; }

        public SetIpdcCode(
            PublicServiceId publicServiceId,
            IpdcCode ipdcCode)
        {
            PublicServiceId = publicServiceId;
            IpdcCode = ipdcCode;
        }
    }
}
