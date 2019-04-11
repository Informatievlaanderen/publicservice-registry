namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Requests
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using NodaTime;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "ChangePeriodOfLifeCycleStage", Namespace = "")]
    public class ChangePeriodOfLifeCycleStageRequest
    {
        /// <summary>
        /// Begindatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 1)]
        public DateTime? Vanaf { get; set; }

        /// <summary>
        /// Einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 2)]
        public DateTime? Tot { get; set; }
    }

    public class ChangePeriodOfLifeCycleStageRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ChangePeriodOfLifeCycleStageRequest
            {
                Vanaf = DateTime.ParseExact("13.03.2019", "dd.MM.yyyy", CultureInfo.InvariantCulture),
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
                    new ValidFrom(message.Vanaf.HasValue ? LocalDate.FromDateTime(message.Vanaf.Value) : (LocalDate?)null),
                    new ValidTo(message.Tot.HasValue ? LocalDate.FromDateTime(message.Tot.Value) : (LocalDate?)null)));
        }
    }
}
