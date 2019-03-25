namespace PublicServiceRegistry.PublicService.Commands
{
    using System.Collections.Generic;

    public class UpdateLabels
    {
        public PublicServiceId PublicServiceId { get; }

        public Dictionary<LabelType, LabelValue> Labels { get; }

        public UpdateLabels(
            PublicServiceId publicServiceId,
            Dictionary<LabelType, LabelValue> labels)
        {
            PublicServiceId = publicServiceId;
            Labels = labels;
        }
    }
}
