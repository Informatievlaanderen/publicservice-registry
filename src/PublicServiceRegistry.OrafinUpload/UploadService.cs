namespace PublicServiceRegistry.OrafinUpload
{
    public class UploadService
    {
        private readonly IFtpConnection _ftpConnection;
        private readonly IOrafinFormatter _orafinFormatter;
        private readonly IPublicServiceRepository _publicServiceRepository;

        public UploadService(
            IFtpConnection ftpConnection,
            IOrafinFormatter orafinFormatter,
            IPublicServiceRepository publicServiceRepository)
        {
            _ftpConnection = ftpConnection;
            _orafinFormatter = orafinFormatter;
            _publicServiceRepository = publicServiceRepository;
        }

        public void Run()
        {
            // TODO: Make configurable
            var fileName = new FileName("Orafin", "txt");
            var subsidies = _publicServiceRepository.GetActiveSubsidies();
            var fileContent = _orafinFormatter.FormatList(subsidies);

            _ftpConnection.UploadFile(fileName, fileContent);
        }
    }
}
