namespace PublicServiceRegistry.Api.Backoffice.LifeCycle
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.ETag;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Pagination;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Converters;
    using Projections.Backoffice;
    using Queries;
    using Requests;
    using Responses;
    using Security;
    using Swashbuckle.AspNetCore.Filters;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen/{id}/levensloop")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class LifeCycleController : ApiBusController
    {
        public LifeCycleController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Vraag de levensloop van een dienstverlening op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de opvraging van levensloop gelukt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet("fases")]
        [ProducesResponseType(typeof(List<LifeCycleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PreconditionFailedResult), StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LifeCycleResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> List(
            [FromServices] BackofficeContext context,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var filter = Request.ExtractFilteringRequest<LifeCycleFilter>();
            var sorting = Request.ExtractSortingRequest();
            var pagination = Request.ExtractPaginationRequest();

            var pagedLifeCycle =
                new LifeCycleQuery(context, id)
                    .Fetch(filter, sorting, pagination);

            Response.AddPagedQueryResultHeaders(pagedLifeCycle);

            return Ok(await pagedLifeCycle.Items.ToListAsync(cancellationToken));
        }

        /// <summary>
        /// Voeg een levensfase toe aan de levenscyclus van een dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("fases")]
        [ProducesResponseType(typeof(List<AcceptedResult>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<BadRequestObjectResult>), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(AddStageToLifeCycleRequest), typeof(AddStageToLifeCycleRequestExample))]
        public async Task<IActionResult> Put(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] AddStageToLifeCycleRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(request))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    AddStageToLifeCycleRequestMapping.Map(id, request),
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// Voeg een levensfase toe aan de levenscyclus van een dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="faseId">Id van de bestaande dienstverlening.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("fases/{faseId}")]
        [ProducesResponseType(typeof(List<AcceptedResult>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(List<BadRequestObjectResult>), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(ChangePeriodOfLifeCycleStageRequest), typeof(ChangePeriodOfLifeCycleStageRequestExample))]
        public async Task<IActionResult> Put(
            [FromCommandId] Guid? commandId,
            [FromRoute] string id,
            [FromRoute] string faseId,
            [FromBody] ChangePeriodOfLifeCycleStageRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(request))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId ?? Guid.NewGuid(),
                    ChangePeriodOfLifeCycleStageRequestMapping.Map(id, request),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}