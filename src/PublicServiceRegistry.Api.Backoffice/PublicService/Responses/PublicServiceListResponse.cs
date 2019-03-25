namespace PublicServiceRegistry.Api.Backoffice.PublicService.Responses
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Filters;

    public class PublicServiceListResponse
    {
        /// <summary>Id van de dienstverlening.</summary>
        public string Id { get; }

        /// <summary>Naam van de dienstverlening.</summary>
        public string Naam { get; }

        /// <summary>Code van de verantwoordelijke autoriteit</summary>
        public string VerantwoordelijkeAutoriteitCode { get; }

        /// <summary>Naam van de verantwoordelijke autoriteit</summary>
        public string VerantwoordelijkeAutoriteitNaam { get; }

        /// <summary>Of de dienstverlening al dan niet naar Orafin geexporteerd wordt.</summary>
        public bool ExportNaarOrafin { get; }

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
