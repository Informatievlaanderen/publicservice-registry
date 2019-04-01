namespace PublicServiceRegistry.Api.Backoffice.LifeCycle.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Levensloopfase", Namespace = "")]
    public class PublicServiceLifeCycleResponseItem
    {
        /// <summary>
        /// Id van de dienstverlening
        /// </summary>
        [DataMember(Name = "PublicServiceId", Order = 1)]
        public string PublicServiceId { get; }

        /// <summary>
        /// Locale id van de levensloopfase
        /// </summary>
        [DataMember(Name = "LocalId", Order = 5)]
        public int LocalId { get; }

        /// <summary>
        /// Id van het type van de levensloopfase
        /// </summary>
        [DataMember(Name = "LifeCycleStageTypeId", Order = 5)]
        public string LifeCycleStageTypeId { get; }

        /// <summary>
        /// Naam van het type van de levensloopfase
        /// </summary>
        [DataMember(Name = "LifeCycleStageTypeName", Order = 5)]
        public string LifeCycleStageTypeName { get; set; }

        /// <summary>
        /// Startdatum van de levensloopfase
        /// </summary>
        [DataMember(Name = "From", Order = 5)]
        public DateTime? From { get; }

        /// <summary>
        /// Einddatum van de levensloopfase
        /// </summary>
        [DataMember(Name = "To", Order = 5)]
        public DateTime? To { get; }


        public PublicServiceLifeCycleResponseItem(string publicServiceId, int localId, string lifeCycleStageTypeId, DateTime? from, DateTime? to)
        {
            PublicServiceId = publicServiceId;
            LocalId = localId;
            LifeCycleStageTypeId = lifeCycleStageTypeId;
            LifeCycleStageTypeName = PublicServiceRegistry.LifeCycleStageType.Parse(lifeCycleStageTypeId).Translation.Name;
            From = from;
            To = to;
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
                    DateTime.Now,
                    DateTime.Now.AddDays(1)),

                new PublicServiceLifeCycleResponseItem(
                    "DVR000000001",
                    2,
                    "Planned",
                    DateTime.Now.AddDays(3),
                    DateTime.Now.AddDays(4)),
            };
        }
    }
}
