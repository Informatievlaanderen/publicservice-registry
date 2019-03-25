namespace PublicServiceRegistry.Api.Backoffice.Security
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using IdentityModel;
    using Microsoft.IdentityModel.Tokens;

    public interface IPublicServiceRegistryTokenBuilder
    {
        string BuildJwt(ClaimsIdentity identity);

        ClaimsIdentity ParseRoles(ClaimsIdentity identity);
    }

    public class PublicServiceRegistryTokenBuilder : IPublicServiceRegistryTokenBuilder
    {
        private readonly OpenIdConnectConfiguration _configuration;

        public PublicServiceRegistryTokenBuilder(OpenIdConnectConfiguration configuration) => _configuration = configuration;

        public string BuildJwt(ClaimsIdentity identity)
        {
            var plainTextSecurityKey = _configuration.JwtSharedSigningKey;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration.JwtAudience,
                Issuer = _configuration.JwtIssuer,
                Subject = identity,
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTimeOffset.FromUnixTimeSeconds(long.Parse(identity.GetClaim(JwtClaimTypes.Expiration))).UtcDateTime,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }

        public ClaimsIdentity ParseRoles(ClaimsIdentity identity)
        {
            // todo: not sure we need to translate the claims.
            // userid
            identity.AddOrUpdateClaim(
                PublicServiceRegistryClaims.ClaimUserId,
                new Claim(
                    PublicServiceRegistryClaims.ClaimUserId,
                    identity.GetClaim(PublicServiceRegistryClaims.ClaimAcmId),
                    ClaimValueTypes.String));

            // name
            identity.AddOrUpdateClaim(
                ClaimTypes.Name,
                new Claim(
                    ClaimTypes.Name,
                    identity.GetClaim(JwtClaimTypes.FamilyName),
                    ClaimValueTypes.String));

            var roles = identity.GetClaims(PublicServiceRegistryClaims.ClaimRoles)
                .Select(x => x.ToLowerInvariant())
                .Where(x => x.StartsWith(PublicServiceRegistryClaims.PublicServiceRegistryPrefix))
                .ToList();

            var developers = _configuration.Developers?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLowerInvariant());
            if (developers != null && developers.Contains(identity.GetClaim(PublicServiceRegistryClaims.ClaimAcmId).ToLowerInvariant()))
                AddRoleClaim(identity, PublicserviceRegistryApiClaims.DeveloperRole);

            if (roles.Count <= 0)
                return identity;

            /* ROLES!!!
             * PublicServiceRegistryBeheerder-algemeenbeheerder,beheerder:OVO002949
             * PublicServiceRegistryBeheerder-beheerder:OVO001831
             * PublicServiceRegistryBeheerder-beheerder:OVO001834
             * PublicServiceRegistryBeheerder-orgaanbeheerder:OVO001834
             * ==> mag alle organen beheren, OVO nummer is puur technisch en nutteloos.
             */

            if (roles.Any(x => x.Contains(PublicServiceRegistryClaims.PublicServiceRegistryAdminRole)))
            {
                // super admin
                AddRoleClaim(identity, PublicserviceRegistryApiClaims.AdminRole);
            }
            else
            {
                if (roles.Any(x => x.Contains(PublicServiceRegistryClaims.CentraleBeheerderRole)))
                    AddRoleClaim(identity, PublicserviceRegistryApiClaims.CentraleBeheerderRole);

                if (!roles.Any(x => x.Contains(PublicServiceRegistryClaims.BeheerderRole)))
                    return identity;

                AddRoleClaim(identity, PublicserviceRegistryApiClaims.BeheerderRole);

                var adminRoles = roles.Where(x => x.Contains(PublicServiceRegistryClaims.BeheerderRole));
                foreach (var role in adminRoles)
                    AddOrganisationClaim(
                        identity,
                        role.Replace(PublicServiceRegistryClaims.PublicServiceRegistryPrefix, string.Empty).Split(':')[1]);
            }

            return identity;
        }

        private static void AddRoleClaim(ClaimsIdentity identity, string value)
        {
            var claim = new Claim(ClaimTypes.Role, value, ClaimValueTypes.String);
            if (!identity.HasClaim(ClaimTypes.Role, value))
                identity.AddClaim(claim);
        }

        private static void AddOrganisationClaim(ClaimsIdentity identity, string value)
        {
            var claim = new Claim(PublicServiceRegistryClaims.ClaimOrganisation, value, ClaimValueTypes.String);
            if (!identity.HasClaim(ClaimTypes.Role, value))
                identity.AddClaim(claim);
        }
    }
}
