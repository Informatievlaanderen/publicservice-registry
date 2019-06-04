namespace PublicServiceRegistry.Api.Backoffice.PublicService.Responses
{
    using System.Runtime.Serialization;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using LifeCycleStageType = PublicServiceRegistry.LifeCycleStageType;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [DataContract(Name = "Dienstverlening", Namespace = "")]
    public class PublicServiceResponse
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

        /// <summary>Huidige levensloopfase type.</summary>
        [DataMember(Name = "HuidigeLevensloopfaseType", Order = 6)]
        public string CurrentLifeCycleStageType { get; }

        /// <summary>Naam van het huidige levensloopfase type.</summary>
        [DataMember(Name = "HuidigeLevensloopfaseTypeNaam", Order = 7)]
        public string CurrentLifeCycleStageTypeName { get; }


        /// <summary>Corresponderende Ipdc code.</summary>
        [DataMember(Name = "IpdcCode", Order = 8)]
        public string IpdcCode { get; }

        public PublicServiceResponse(
            string id,
            string name,
            string competentAuthorityCode,
            string competentAuthorityName,
            bool exportToOrafin,
            string currentLifeCycleStageType,
            string currentLifeCycleStageTypeName,
            string ipdcCode)
        {
            Id = id;
            Naam = name;
            VerantwoordelijkeAutoriteitCode = competentAuthorityCode;
            VerantwoordelijkeAutoriteitNaam = competentAuthorityName;
            ExportNaarOrafin = exportToOrafin;
            CurrentLifeCycleStageType = currentLifeCycleStageType;
            CurrentLifeCycleStageTypeName = currentLifeCycleStageTypeName;
            IpdcCode = ipdcCode;
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
                true,
                LifeCycleStageType.PhasingOut,
                LifeCycleStageType.PhasingOut.Translation.Name,
                "1234");
        }
    }

    public class PublicServiceNotFoundResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProblemDetails
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = ProblemDetails.DefaultTitle,
                Detail = "Onbestaande dienstverlening.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
        }
    }
}
