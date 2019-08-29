namespace PublicServiceRegistry.Api.Backoffice.Security
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [ApiVersion("2.0")]
    [AdvertiseApiVersions("2.0")]
    [ApiRoute("security")]
    [ApiExplorerSettings(GroupName = "Beveiliging")]
    public class SecurityController : ApiController
    {
        private readonly OpenIdConnectConfiguration _openIdConnectConfiguration;
        private readonly OIDCAuthAcmConfiguration _acmAuthConfiguration;

        public SecurityController(
            IOptions<OpenIdConnectConfiguration> authOptions,
            IOptions<OIDCAuthAcmConfiguration> acmAuthOptions)
        {
            _openIdConnectConfiguration = authOptions.Value;
            _acmAuthConfiguration = acmAuthOptions.Value;
        }

        [HttpGet]
        [PublicServiceRegistryAuthorize]
        public IActionResult Get()
        {
            var user = User ?? new ClaimsPrincipal();

            var firstName = user.GetClaim(ClaimTypes.GivenName);
            var name = user.GetClaim(ClaimTypes.Name);

            return Ok(new
            {
                firstName,
                name,
            });
        }

        [HttpGet("info")]
        public IActionResult Info()
        {
            return Ok(_acmAuthConfiguration);
        }

        [HttpGet("signin")]
        public IActionResult SignIn(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return BadRequest();

            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                    IsPersistent = true
                },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("exchange")]
        public async Task<IActionResult> ExchangeAsync(string code, string state)
        {
            var tokenClient = new TokenClient(
                $"{_openIdConnectConfiguration.Authority}{_openIdConnectConfiguration.TokenEndPoint}",
                _openIdConnectConfiguration.ClientId,
                _openIdConnectConfiguration.ClientSecret);

            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(
                code,
                new Uri(
                    _openIdConnectConfiguration.AuthorizationRedirectUri,
                    UriKind.RelativeOrAbsolute).ToString());

            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error);

            var token = new JwtSecurityToken(tokenResponse.IdentityToken);
            var identity = new ClaimsIdentity();
            identity.AddClaims(token.Claims);

            var publicServiceRegistryTokenBuilder = new PublicServiceRegistryTokenBuilder(_openIdConnectConfiguration);
            identity = publicServiceRegistryTokenBuilder.ParseRoles(identity);
            var jwtToken = publicServiceRegistryTokenBuilder.BuildJwt(identity);

            return Ok(jwtToken);
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            var returnUrl = _openIdConnectConfiguration.SignOutReturnUrl.ToLowerInvariant();

            // set cookie
            Response.Cookies.Append(
                _openIdConnectConfiguration.JwtCookieName,
                string.Empty,
                new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _openIdConnectConfiguration.JwtCookieDomain,
                    Secure = _openIdConnectConfiguration.JwtCookieSecure,
                    Expires = DateTimeOffset.Now.AddDays(-1)
                });

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(returnUrl);
        }

        [HttpGet("debug")]
        public IActionResult Debug()
        {
            if (Request.Cookies[_openIdConnectConfiguration.JwtCookieName] == null)
                return NotFound();

            var i = 0;
            return Ok(
                User
                    .Claims
                    .ToDictionary(claim => $"{++i} {claim.Type}", claim => claim.Value));
        }
    }
}
