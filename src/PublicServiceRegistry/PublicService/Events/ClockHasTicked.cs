namespace PublicServiceRegistry.PublicService.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;

    [EventName("ClockHasTicked")]
    [EventDescription("Duidt aan dat een bepaald tijdstip is gepasseerd.")]
    public class ClockHasTicked
    {
        public DateTime DateTime { get; }

        public ClockHasTicked(DateTime dateTime) => DateTime = dateTime;
    }
}
