namespace PublicServiceRegistry.OrafinUpload.Tests
{
    using System;
    using FluentAssertions;
    using OrafinUpload;
    using Projections.Backoffice.PublicServiceList;
    using Xunit;
    using Xunit.Categories;

    public class OrafinFormatTests
    {
        private readonly IOrafinFormatter _sut;

        public OrafinFormatTests() => _sut = new OrafinFormatter();

        [Fact]
        [UnitTest]
        public void WhenFormattingAnEmptyListThenTheHeaderIsReturned()
        {
            _sut
                .FormatList(null)
                .Should().Be("identificator|naam|PubliekeOrganisatie|startdatum|einddatum|");
        }

        [Fact]
        [UnitTest]
        public void WhenFormattingAListWithOneItemThenTheSecondLineIsTheFormattedItem()
        {
            var id = "DVR000000001";
            var name = "KidsID aanvragen";
            var organisation = "OVO000108";
            var service = new PublicServiceListItem
            {
                PublicServiceId = id,
                Name = name,
                CompetentAuthorityCode = organisation
            };

            _sut
                .FormatList(new[] { service })
                .Split(Environment.NewLine)
                .Should().ContainInOrder(
                    "identificator|naam|PubliekeOrganisatie|startdatum|einddatum|",
                    $"{id}|{name}|{organisation}|||");
        }
    }
}
