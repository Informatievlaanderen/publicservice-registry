namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using System;

    public class OpenIdConnectCredentialsNotProvided : Exception
    {
        public OpenIdConnectCredentialsNotProvided() :
            base("ClientId and ClientSecret should be provided. Use dotnet user-secrets to provide them.") { }
    }
}
