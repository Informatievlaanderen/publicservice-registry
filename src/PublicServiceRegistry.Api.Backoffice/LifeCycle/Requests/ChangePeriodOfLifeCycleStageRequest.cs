namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Requests
{
    using System;
    using System.Globalization;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class ChangePeriodOfLifeCycleStageRequest
    {
        /// <summary>
        /// Begindatum van de levensloopfase (inclusief).
        /// </summary>
        public DateTime? Van { get; set; }

        /// <summary>
        /// Einddatum van de levensloopfase (inclusief).
        /// </summary>
        public DateTime? Tot { get; set; }
    }

    public class ChangePeriodOfLifeCycleStageRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ChangePeriodOfLifeCycleStageRequest
            {
                Van = DateTime.ParseExact("13.03.2019", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Tot = DateTime.ParseExact("13.03.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            };
        }
    }

    public static class ChangePeriodOfLifeCycleStageRequestMapping
    {
        public static ChangePeriodOfLifeCycleStage Map(string id, int faseId, ChangePeriodOfLifeCycleStageRequest message)
        {
            return new ChangePeriodOfLifeCycleStage(
                new PublicServiceId(id),
                faseId,
                new LifeCycleStagePeriod(
                    new ValidFrom(message.Van),
                    new ValidTo(message.Tot)));
        }
    }
}
