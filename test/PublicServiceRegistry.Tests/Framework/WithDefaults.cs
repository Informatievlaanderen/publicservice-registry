namespace PublicServiceRegistry.Tests.Framework
{
    using AutoFixture;

    public class WithDefaults : CompositeCustomization
    {
        public WithDefaults() : base(
            new WithPublicServiceId(),
            new WithPublicServiceName(),
            new WithPrivateZoneId(),
            new WithOvoNumber(),
            new WithOrganisation(),
            new WithLabelType(),
            new WithLabelValue(),
            new WithLifeCycleStage(),
            new WithLifeCycleStagePeriod(),
            new WithLifeCycleStageId(),
            new WithIpdcCode(),
            new WithLegislativeDocumentId()) { }
    }
}
