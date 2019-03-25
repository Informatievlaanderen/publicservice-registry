namespace PublicServiceRegistry.Api.Backoffice.Security
{
    public class OIDCAuthAcmConfiguration
    {
        public static string Section = "OIDCAuthAcm";

        public string Authority { get; set; }

        public string Issuer { get; set; }

        public string AuthorizationEndpoint { get; set; }

        public string UserInfoEndPoint { get; set; }

        public string EndSessionEndPoint { get; set; }

        public string JwksUri { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string PostLogoutRedirectUri { get; set; }

    }
}
