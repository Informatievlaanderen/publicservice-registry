namespace PublicServiceRegistry.OrafinUpload
{
    public class FtpConfiguration
    {
        public const string SectionName = "FtpConfiguration";

        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string RemotePath { get; set; }

        public bool IsSet => !string.IsNullOrWhiteSpace(Host) &&
                             !string.IsNullOrWhiteSpace(User) &&
                             !string.IsNullOrWhiteSpace(Password) &&
                             null != RemotePath;
    }
}
