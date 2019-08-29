namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using PublicServiceLabelList;
    using PublicService.Events;
    using Xunit;

    public class PublicServiceLabelListTests
    {
        private readonly Fixture _fixture;

        public PublicServiceLabelListTests()
        {
            _fixture = new Fixture();
            _fixture.CustomizeLabelWasAssigned();
        }

        [Fact]
        public Task WhenLabelWasAssigned()
        {
            var random = new Random();
            var data = _fixture.CreateMany<LabelWasAssigned>(random.Next(1, 100))
                .Select(labelWasAssigned =>
                {
                    var expected = CreateListItemFromEvent(labelWasAssigned);

                    return new
                    {
                        @event = labelWasAssigned,
                        expected
                    };
                }).ToList();

            return new PublicServiceLabelListProjections()
                .Scenario()
                .Given(data.Select(d => d.@event))
                .Expect(data
                    .Select(d => d.expected)
                    .Cast<object>()
                    .ToArray());
        }

        [Fact]
        public Task WhenLabelWasAssignedTwice()
        {
            var random = new Random();
            var data = _fixture.CreateMany<LabelWasAssigned>(random.Next(1, 100))
                .Select(labelWasAssigned =>
                {
                    var expected = CreateListItemFromEvent(labelWasAssigned);

                    return new
                    {
                        @event = labelWasAssigned,
                        expected
                    };
                }).ToList();

            var labelXAssigned = _fixture.Create<LabelWasAssigned>();
            var labelXAssignedAgain = new LabelWasAssigned(
                new PublicServiceId(labelXAssigned.PublicServiceId),
                LabelType.Parse(labelXAssigned.LabelType),
                _fixture.Create<LabelValue>());

            var events = data
                .Select(d => d.@event)
                .Append(labelXAssigned)
                .Append(labelXAssignedAgain);

            var expectedResults = data
                .Select(d => d.expected)
                .Append(CreateListItemFromEvent(labelXAssignedAgain));

            return new PublicServiceLabelListProjections()
                .Scenario()
                .Given(events)
                .Expect(expectedResults
                    .Cast<object>()
                    .ToArray());
        }

        [Fact]
        public Task WhenPublicServiceWasRemoved()
        {
            var labelWasAssigned = _fixture.Create<LabelWasAssigned>();
            var anotherLabelWasAssigned = _fixture.Create<LabelWasAssigned>();
            var publicServiceWasRemoved = new PublicServiceWasRemoved(new PublicServiceId(labelWasAssigned.PublicServiceId), new ReasonForRemoval("because"));

            return new PublicServiceLabelListProjections()
                .Scenario()
                .Given(labelWasAssigned, anotherLabelWasAssigned, publicServiceWasRemoved)
                .Expect(CreateListItemFromEvent(anotherLabelWasAssigned));
        }

        private static PublicServiceLabelListItem CreateListItemFromEvent(LabelWasAssigned labelWasAssigned) =>
            new PublicServiceLabelListItem
            {
                LabelType = labelWasAssigned.LabelType,
                LabelValue = labelWasAssigned.LabelValue,
                PublicServiceId = labelWasAssigned.PublicServiceId
            };
    }
}
