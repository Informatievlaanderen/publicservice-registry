namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Requests
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using NodaTime;
    using PublicServiceRegistry.PublicService.Commands;
    using Swashbuckle.AspNetCore.Filters;
    using LifeCycleStageType = PublicServiceRegistry.LifeCycleStageType;

    [DataContract(Name = "AddStageToLifeCycle", Namespace = "")]
    public class AddStageToLifeCycleRequest
    {
        /// <summary>
        /// Type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseType", Order = 1)]
        public string LevensloopfaseType { get; set; }

        /// <summary>
        /// Begindatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 2)]
        public DateTime? Vanaf { get; set; }

        /// <summary>
        /// Einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 3)]
        public DateTime? Tot { get; set; }
    }

    public class AddStageToLifeCycleRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AddStageToLifeCycleRequest
            {
                LevensloopfaseType = LifeCycleStageType.Active.ToString(),
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
                LifeCycleStageType.Parse(message.LevensloopfaseType),
                new LifeCycleStagePeriod(
                    new ValidFrom(message.Vanaf.HasValue ? LocalDate.FromDateTime(message.Vanaf.Value) : (LocalDate?)null),
                    new ValidTo(message.Tot.HasValue ? LocalDate.FromDateTime(message.Tot.Value) : (LocalDate?)null)));
    }
}
