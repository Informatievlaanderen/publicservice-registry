namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using NodaTime;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloopfase", Namespace = "")]
    public class PublicServiceLifeCycleResponseItem
    {
        /// <summary>
        /// Id van de dienstverlening.
        /// </summary>
        [DataMember(Name = "DienstverleningId", Order = 1)]
        public string DienstverleningId { get; private set; }

        /// <summary>
        /// Id van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseId", Order = 2)]
        public int LevensloopfaseId { get; private set; }

        /// <summary>
        /// Id van het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseTypeId", Order = 3)]
        public string LevensloopfaseTypeId { get; private set; }

        /// <summary>
        /// Naam van het type van de levensloopfase.
        /// </summary>
        [DataMember(Name = "LevensloopfaseTypeNaam", Order = 4)]
        public string LevensloopfaseTypeNaam { get; private set; }

        /// <summary>
        /// Startdatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Vanaf", Order = 5)]
        public string Vanaf { get; private set; }

        /// <summary>
        /// Einddatum van de levensloopfase (inclusief).
        /// </summary>
        [DataMember(Name = "Tot", Order = 6)]
        public string Tot { get; private set; }


        public PublicServiceLifeCycleResponseItem(string publicServiceId, int localId, string lifeCycleStageTypeId, LocalDate? from, LocalDate? to)
        {
            DienstverleningId = publicServiceId;
            LevensloopfaseId = localId;
            LevensloopfaseTypeId = lifeCycleStageTypeId;
            LevensloopfaseTypeNaam = PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageTypeId).Translation.Name;
            Vanaf = from?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Tot = to?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }


    public class PublicServiceLifeCycleResponseItemExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<PublicServiceLifeCycleResponseItem>
            {
                new PublicServiceLifeCycleResponseItem(
                    "DVR000000001",
                    1,
                    "Active",
                    LocalDate.FromDateTime(DateTime.Now),
                    LocalDate.FromDateTime(DateTime.Now.AddDays(1))),

                new PublicServiceLifeCycleResponseItem(
                    "DVR000000001",
                    2,
                    "Planned",
                    LocalDate.FromDateTime(DateTime.Now.AddDays(3)),
                    LocalDate.FromDateTime(DateTime.Now.AddDays(4))),
            };
        }
    }
}
