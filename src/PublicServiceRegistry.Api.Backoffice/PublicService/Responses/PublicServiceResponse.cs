namespace PublicServiceRegistry.Api.Backoffice.PublicService.Responses
{
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class PublicServiceResponse
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

        public PublicServiceResponse(
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

    public class PublicServiceResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new PublicServiceResponse(
                "DVR000000002",
                "Schooltoelage voor het basisonderwijs en het secundair onderwijs",
                "OVO001951",
                "Agentschap voor Hoger Onderwijs, Volwassenenonderwijs, Kwalificaties en Studietoelagen",
                true);
        }
    }

    public class PublicServiceNotFoundResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = BasicApiProblem.DefaultTitle,
                Detail = "Onbestaande dienstverlening.",
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
