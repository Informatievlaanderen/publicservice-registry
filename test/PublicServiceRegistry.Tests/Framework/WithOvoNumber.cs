namespace PublicServiceRegistry.Tests.Framework
{
    using System;
    using AutoFixture;
    using PublicService;

    public class WithOvoNumber : ICustomization
    {
        private readonly int? _fixedNumber;

        public WithOvoNumber(int? fixedNumber = null) => _fixedNumber = fixedNumber;

        public void Customize(IFixture fixture)
        {
            fixture.Customize<OvoNumber>(c => c.FromFactory(() => new OvoNumber("OVO" + (_fixedNumber ?? fixture.Create<int>()))));

            fixture.Customize<Organisation>(c => c.FromFactory(() => new Organisation
            {
                Name = new OrganisationName(fixture.Create<string>()),
                OvoNumber = fixture.Create<OvoNumber>(),
                Provenance = OrganisationProvenance.From(OrganisationSource.DataVlaanderen, fixture.Create<Uri>().ToString())
            }));
        }
    }
}
