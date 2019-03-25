namespace PublicServiceRegistry.PublicService
{
    public class Organisation
    {
        public OrganisationName Name { get; set; }
        public OvoNumber OvoNumber { get; set; }
        public OrganisationProvenance Provenance { get; set; }
    }

    public class OrganisationProvenance
    {
        public OrganisationSource Source { get; }
        public string Uri { get; }

        private OrganisationProvenance(
            OrganisationSource source,
            string uri)
        {
            Source = source;
            Uri = uri;
        }

        public static OrganisationProvenance FromDataVlaanderen(string sourceUri) => new OrganisationProvenance(OrganisationSource.DataVlaanderen, sourceUri);

        public static OrganisationProvenance FromWegwijsSearch(string sourceUri) => new OrganisationProvenance(OrganisationSource.WegwijsSearch, sourceUri);

        public static OrganisationProvenance From(OrganisationSource source, string organisationUri) => new OrganisationProvenance(source, organisationUri);
    }

    public struct OrganisationSource
    {
        public static OrganisationSource DataVlaanderen = new OrganisationSource("DataVlaanderen");
        public static OrganisationSource WegwijsSearch = new OrganisationSource("WegwijsSearch");
        public static OrganisationSource Unknown = new OrganisationSource("Unknown");

        public string Source { get; }

        private OrganisationSource(string source) => Source = source;

        public static OrganisationSource Parse(string source) =>
            string.IsNullOrEmpty(source)
                ? Unknown
                : new OrganisationSource(source);

        public static implicit operator string(OrganisationSource source) => source.Source;
    }
}
