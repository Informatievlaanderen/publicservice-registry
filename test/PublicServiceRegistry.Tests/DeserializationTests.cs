namespace PublicServiceRegistry.Tests
{
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using FluentAssertions;
    using Newtonsoft.Json;
    using PublicService.Events;
    using Xunit;
    using OrganisationSource = PublicService.OrganisationSource;

    public class DeserializationTests
    {
        /// <summary>
        /// The CompetentAuthorityProvenance property was added after we went into production,
        /// so some events are already serialized in the database without this property.
        /// This tests verifies whether deserializing these events happens correctly.
        /// </summary>
        [Fact]
        public void DeserializingWithoutSourceProperties()
        {
            var serialized = @"{""publicServiceId"":""DVR000000001"",""name"":""dag wouter"",""privateZoneId"":""UNREGISTERED""}";

            var settings = EventsJsonSerializerSettingsProvider.CreateSerializerSettings();

            var @event = JsonConvert.DeserializeObject<CompetentAuthorityWasAssigned>(serialized, settings);

            @event.CompetentAuthorityProvenance.Uri.Should().BeNull();
            @event.CompetentAuthorityProvenance.Source.Should().Be(OrganisationSource.Unknown);
        }
    }
}
