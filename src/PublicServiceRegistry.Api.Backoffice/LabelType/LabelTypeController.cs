namespace PublicServiceRegistry.Api.Backoffice.LabelType
{
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Converters;
    using Projections.Backoffice;
    using Responses;
    using Security;
    using Swashbuckle.AspNetCore.Filters;
    using LabelType = PublicServiceRegistry.LabelType;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("alternatievebenamingstypes")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class LabelTypeController : ApiController
    {
        /// <summary>
        /// Vraag een lijst met dienstverleningen op.
        /// </summary>
        /// <param name="context"></param>
        /// <response code="200">Als de opvraging van een lijst met dienstverlening gelukt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<LabelTypeListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LabelTypeListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public IActionResult List(
            [FromServices] BackofficeContext context) =>
            Ok(LabelType.All.Select(x => new LabelTypeListResponse(x)));
    }
}
