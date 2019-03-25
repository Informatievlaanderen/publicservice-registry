namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using AutoFixture;
    using AutoFixture.Dsl;
    using PublicServiceRegistry.PublicService.Events;

    internal static class Customizations
    {
        public static IPostprocessComposer<T> FromFactory<T>(this IFactoryComposer<T> composer, Func<Random, T> factory) =>
            composer.FromFactory<int>(value => factory(new Random(value)));

        public static void CustomizePublicServiceWasRegistered(this IFixture fixture) =>
            fixture.Customize<PublicServiceWasRegistered>(customization =>
                customization
                    .FromFactory(generator =>
                        new PublicServiceWasRegistered(
                            PublicServiceId.FromNumber(fixture.Create<int>()),
                            new PublicServiceName(fixture.Create<string>()),
                            PrivateZoneId.Unregistered
                        )).OmitAutoProperties());

        public static void CustomizeLabelWasAssigned(this IFixture fixture) =>
            fixture.Customize<LabelWasAssigned>(customization =>
                customization
                    .FromFactory(generator =>
                        new LabelWasAssigned(
                            PublicServiceId.FromNumber(fixture.Create<int>()),
                            LabelType.All[generator.Next() % LabelType.All.Length],
                            new LabelValue(fixture.Create<string>())
                        )).OmitAutoProperties());
    }
}
