namespace PublicServiceRegistry.PublicService.Events
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("CompetentAuthorityWasAssigned")]
    [EventDescription("De organisatie werd aangewezen als verantwoordelijke autoriteit.")]
    public class CompetentAuthorityWasAssigned
    {
        public string PublicServiceId { get; }
        public string CompetentAuthorityCode { get; }
        public string CompetentAuthorityName { get; }
        public OrganisationProvenance CompetentAuthorityProvenance { get; }

        public CompetentAuthorityWasAssigned(
            PublicServiceId publicServiceId,
            OvoNumber competentAuthorityCode,
            OrganisationName competentAuthorityName,
            PublicServiceRegistry.PublicService.OrganisationProvenance competentAuthorityProvenance)
        {
            PublicServiceId = publicServiceId;
            CompetentAuthorityCode = competentAuthorityCode;
            CompetentAuthorityName = competentAuthorityName;
            CompetentAuthorityProvenance =
                new OrganisationProvenance(
                    competentAuthorityProvenance.Source,
                    competentAuthorityProvenance.Uri);
        }

        [JsonConstructor]
        private CompetentAuthorityWasAssigned(
            string publicServiceId,
            string competentAuthorityCode,
            string competentAuthorityName,
            OrganisationProvenance competentAuthorityProvenance) :
            this(
                new PublicServiceId(publicServiceId),
                new OvoNumber(competentAuthorityCode),
                new OrganisationName(competentAuthorityName),
                PublicServiceRegistry.PublicService.OrganisationProvenance.From(
                    OrganisationSource.Parse(competentAuthorityProvenance.Source),
                    competentAuthorityProvenance.Uri)) { }
    }

    public struct OrganisationProvenance
    {
        public string Source { get; }
        public string Uri { get; }

        public OrganisationProvenance(
            OrganisationSource organisationSource,
            string organisationUri)
        {
            Source = organisationSource;
            Uri = organisationUri;
        }

        [JsonConstructor]
        private OrganisationProvenance(
            string source,
            string uri) :
            this(
                OrganisationSource.Parse(source),
                uri) { }
    }
}
