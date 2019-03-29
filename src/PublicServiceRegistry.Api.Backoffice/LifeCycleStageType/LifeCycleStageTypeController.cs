namespace PublicServiceRegistry.Api.Backoffice.LifeCycleStageType
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
    using LifeCycleStageType = PublicServiceRegistry.LifeCycleStageType;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("levensloopfasetypes")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class LifeCycleStageTypeController : ApiController
    {
        /// <summary>
        /// Vraag een lijst met dienstverlening levensloopfases op.
        /// </summary>
        /// <param name="context"></param>
        /// <response code="200">Als de opvraging van een lijst met dienstverlening levensloopfases gelukt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<LifeCycleStageTypeListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LifeCycleStageTypeListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public IActionResult List(
            [FromServices] BackofficeContext context) =>
            Ok(LifeCycleStageType.All.Select(x => new LifeCycleStageTypeListResponse(x)));
    }
}
