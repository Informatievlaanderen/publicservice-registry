namespace PublicServiceRegistry.Api.Backoffice.Security
{
    public class PublicServiceRegistryClaims
    {
        // TODO: adapt things to PSR claims

        public const string ClaimRoles = "iv_dienstverlening_rol_3d";
        public const string ClaimAcmId = "vo_id";

        public const string ClaimOrganisation = "urn:be:vlaanderen:wegwijs:organisation";
        public const string ClaimUserId = "urn:be:vlaanderen:dienstverlening:acmid";

        public const string PublicServiceRegistryPrefix = "dienstverleningsregister-";
        public const string PublicServiceRegistryAdminRole = "dienstverleningsregister-admin";
        public const string CentraleBeheerderRole = "dienstverleningsregister-centralebeheerder";
        public const string BeheerderRole = "dienstverleningsregister-centralebeheerder";
    }

    public class PublicserviceRegistryApiClaims
    {
        public const string BeheerderRole = "dienstverleningsregisterBeheerder";
        public const string AdminRole = "dienstverleningsregisterAdmin";
        public const string CentraleBeheerderRole = "centraleBeheerder";
        public const string DeveloperRole = "developer";
    }
}
