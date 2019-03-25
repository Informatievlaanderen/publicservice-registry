namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Framework;
    using PublicService.Commands;
    using PublicService.Events;
    using PublicService.Exceptions;
    using Xunit;
    using Xunit.Abstractions;

    public class UpdateLabelsTest : AutofacBasedTest
    {
        public UpdateLabelsTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory]
        [DefaultData]
        public void CannotUpdateLabelsOnRemovedPublicService(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            ReasonForRemoval reasonForRemoval,
            LabelType labelType,
            LabelValue labelValue)
        {
            Assert(new Scenario()
                .Given(publicServiceId,
                    new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                    new PublicServiceWasRemoved(publicServiceId, reasonForRemoval))
                .When(new UpdateLabels(publicServiceId, new Dictionary<LabelType, LabelValue>{ { labelType, labelValue } }))
                .Throws(new CannotPerformActionOnRemovedPublicService()));
        }

        [Theory]
        [DefaultData]
        public void WithNullLabels(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new UpdateLabels(publicServiceId, null))
                    .ThenNone());
        }

        [Theory]
        [DefaultData]
        public void WithNoLabels(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new UpdateLabels(publicServiceId, new Dictionary<LabelType, LabelValue>()))
                    .ThenNone());
        }

        [Theory]
        [DefaultData]
        public void WithAValidLabel(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LabelType labelType,
            LabelValue labelValue)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new UpdateLabels(publicServiceId, new Dictionary<LabelType, LabelValue>{ { labelType, labelValue } }))
                    .Then(publicServiceId,
                        new LabelWasAssigned(publicServiceId, labelType, labelValue)));
        }

        [Theory]
        [DefaultData]
        public void WithValidLabels(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            Dictionary<LabelType, LabelValue> labels)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered))
                    .When(new UpdateLabels(publicServiceId, labels))
                    .Then(publicServiceId,
                        // ReSharper disable once CoVariantArrayConversion
                        labels
                            .Select(pair => new LabelWasAssigned(new PublicServiceId(publicServiceId), LabelType.Parse(pair.Key), new LabelValue(pair.Value)))
                            .ToArray()));
        }

        [Theory]
        [DefaultData]
        public void WithLabelValueThatIsUnchanged(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LabelType labelType,
            LabelValue labelValue)
        {
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new LabelWasAssigned(publicServiceId, labelType, labelValue))
                    .When(new UpdateLabels(publicServiceId, new Dictionary<LabelType, LabelValue> { { labelType, labelValue } }))
                    .ThenNone());
        }

        [Theory]
        [DefaultData]
        public void WithLabelValueThatIsUpdatedToEmpty(
            PublicServiceId publicServiceId,
            PublicServiceName publicServiceName,
            LabelType labelType,
            LabelValue labelValue)
        {
            var emptyLabelValue = new LabelValue(string.Empty);
            Assert(
                new Scenario()
                    .Given(publicServiceId,
                        new PublicServiceWasRegistered(publicServiceId, publicServiceName, PrivateZoneId.Unregistered),
                        new LabelWasAssigned(publicServiceId, labelType, labelValue))
                    .When(new UpdateLabels(publicServiceId, new Dictionary<LabelType, LabelValue> { { labelType, emptyLabelValue } }))
                    .Then(publicServiceId,
                        new LabelWasAssigned(publicServiceId, labelType, emptyLabelValue)));
        }
    }
}
