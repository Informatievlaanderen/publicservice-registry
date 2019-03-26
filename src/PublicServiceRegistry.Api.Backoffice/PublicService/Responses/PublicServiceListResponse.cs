namespace PublicServiceRegistry.Api.Backoffice.PublicService.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Dienstverlening", Namespace = "")]
    public class PublicServiceListResponse
    {
        /// <summary>Id van de dienstverlening.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>Naam van de dienstverlening.</summary>
        [DataMember(Name = "Naam", Order = 2)]
        public string Naam { get; set; }

        /// <summary>Code van de verantwoordelijke autoriteit</summary>
        [DataMember(Name = "VerantwoordelijkeAutoriteitCode", Order = 3)]
        public string VerantwoordelijkeAutoriteitCode { get; set; }

        /// <summary>Naam van de verantwoordelijke autoriteit</summary>
        [DataMember(Name = "VerantwoordelijkeAutoriteitNaam", Order = 4)]
        public string VerantwoordelijkeAutoriteitNaam { get; set; }

        /// <summary>Of de dienstverlening al dan niet naar Orafin geexporteerd wordt.</summary>
        [DataMember(Name = "ExportNaarOrafin", Order = 5)]
        public bool ExportNaarOrafin { get; set; }

        public PublicServiceListResponse(
            string id,
            string name,
            string competentAuthorityCode,
            string competentAuthorityName,
            bool exportToOrafin)
        {
            Id = id;
            Naam = name;
            VerantwoordelijkeAutoriteitCode = competentAuthorityCode;
            VerantwoordelijkeAutoriteitNaam = competentAuthorityName;
            ExportNaarOrafin = exportToOrafin;
        }
    }

    public class PublicServiceListResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<PublicServiceListResponse>
            {
                new PublicServiceListResponse(
                    "DVR000000001",
                    "Werkplek duaal leren",
                    "OVO001951",
                    "Agentschap voor Hoger Onderwijs, Volwassenenonderwijs, Kwalificaties en Studietoelagen",
                    true),

                new PublicServiceListResponse(
                    "DVR000000002",
                    "Schooltoelage voor het basisonderwijs en het secundair onderwijs",
                    "OVO001951",
                    "Agentschap voor Hoger Onderwijs, Volwassenenonderwijs, Kwalificaties en Studietoelagen",
                    false)
            };
        }
    }
}
