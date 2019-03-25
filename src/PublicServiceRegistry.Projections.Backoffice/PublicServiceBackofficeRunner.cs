namespace PublicServiceRegistry.Projections.Backoffice
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.Extensions.Logging;
    using PublicService;
    using PublicServiceLabelList;
    using PublicServiceLifeCycle;
    using PublicServiceList;

    public class PublicServiceBackofficeRunner : Runner<BackofficeContext>
    {
        public const string Name = "PublicServiceBackofficeRunner";

        public PublicServiceBackofficeRunner(
            EnvelopeFactory envelopeFactory,
            ILogger<PublicServiceBackofficeRunner> logger) :
            base(
                Name,
                envelopeFactory,
                logger,
                new PublicServiceListProjections(),
                new PublicServiceProjections(),
                new PublicServiceLabelListProjections(),
                new PublicServiceLifeCycleListProjections()) { }
    }
}
