namespace PublicServiceRegistry.Api.Backoffice.Security
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;

    public class PublicServiceRegistryAuthorizeAttribute : AuthorizeAttribute
    {
        public PublicServiceRegistryAuthorizeAttribute()
        {
            AuthenticationSchemes = string.Join(", ", JwtBearerDefaults.AuthenticationScheme);
        }
    }
}
