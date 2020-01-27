namespace PublicServiceRegistry.Api.Backoffice.Labels.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;
    using LabelType = PublicServiceRegistry.LabelType;

    public class UpdateLabelsRequest
    {
        /// <summary>
        /// Alternatieve benamingen.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; }
    }

    public class UpdateLabelsRequestExample : IExamplesProvider<UpdateLabelsRequest>
    {
        public UpdateLabelsRequest GetExamples() =>
            new UpdateLabelsRequest
            {
                Labels = new Dictionary<string, string> { { "Ipdc", "Alternatieve naam" } }
            };
    }

    public static class UpdateLabelsRequestMapping
    {
        public static UpdateLabels Map(string id, UpdateLabelsRequest message) =>
            new UpdateLabels(
                new PublicServiceId(id),
                ToLabels(message.Labels));

        private static Dictionary<LabelType, LabelValue> ToLabels(Dictionary<string, string> messageLabels) =>
            messageLabels
                .ToDictionary(
                    pair => LabelType.Parse(pair.Key),
                    pair => new LabelValue(pair.Value));
    }
}
