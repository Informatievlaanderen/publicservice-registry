namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Requests
{
    using System;
    using System.Globalization;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class AddStageToLifeCycleRequest
    {
        /// <summary>
        /// Levensloopfase.
        /// </summary>
        public string Levensloopfase { get; set; }

        /// <summary>
        /// Begindatum van de levensfase (inclusief).
        /// </summary>
        public DateTime? Vanaf { get; set; }

        /// <summary>
        /// Einddatum van de levensfase (inclusief).
        /// </summary>
        public DateTime? Tot { get; set; }
    }

    public class AddStageToLifeCycleRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AddStageToLifeCycleRequest
            {
                Levensloopfase = PublicServiceRegistry.LifeCycleStage.Active.ToString(),
                Vanaf = DateTime.ParseExact("13.03.2019", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Tot = DateTime.ParseExact("13.03.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            };
        }
    }

    public static class AddStageToLifeCycleRequestMapping
    {
        public static AddStageToLifeCycle Map(string id, AddStageToLifeCycleRequest message) =>
            new AddStageToLifeCycle(
                new PublicServiceId(id),
                PublicServiceRegistry.LifeCycleStage.Parse(message.Levensloopfase),
                new LifeCycleStagePeriod(
                    new ValidFrom(message.Vanaf),
                    new ValidTo(message.Tot)));
    }
}
