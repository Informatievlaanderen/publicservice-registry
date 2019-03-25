namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using AutoFixture;
    using PublicService;

    public class WithOrganisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Organisation>(c => c.FromFactory(() => new Organisation
            {
                Name = new OrganisationName(fixture.Create<string>()),
                OvoNumber = fixture.Create<OvoNumber>(),
                Provenance = OrganisationProvenance.From(OrganisationSource.DataVlaanderen, fixture.Create<Uri>().ToString())
            }));
        }
    }
}
