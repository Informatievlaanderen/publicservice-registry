namespace PublicServiceRegistry
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class OrganisationName : StringValueObject<OrganisationName>
    {
        public OrganisationName([JsonProperty("value")] string organisationName) : base(organisationName) { }
    }
}
