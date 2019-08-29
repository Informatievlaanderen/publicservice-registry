namespace PublicServiceRegistry.Projections.Backoffice
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.Extensions.Logging;
    using PublicService;
    using PublicServiceLabelList;
    using PublicServiceLifeCycle;
    using PublicServiceList;

    // TODO: This class can be removed
    public class PublicServiceBackofficeRunner : Runner<BackofficeContext>
    {
        // TODO: With the new projector this is probably wrong
        public const string Name = "PublicServiceBackofficeRunner";

        public PublicServiceBackofficeRunner(
            EnvelopeFactory envelopeFactory,
            ILogger<PublicServiceBackofficeRunner> logger) :
            base(
                Name,
                envelopeFactory,
                logger,
                new PublicServiceListProjections(new ClockProvider()),
                new PublicServiceProjections(),
                new PublicServiceLabelListProjections(),
                new PublicServiceLifeCycleListProjections()) { }
    }
}
