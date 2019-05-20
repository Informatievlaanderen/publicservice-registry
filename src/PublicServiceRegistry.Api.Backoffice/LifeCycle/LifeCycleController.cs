namespace PublicServiceRegistry.Api.Backoffice.LifeCycle
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Pagination;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Infrastructure.ETag;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Converters;
    using Projections.Backoffice;
    using Projections.Backoffice.PublicServiceLifeCycle;
    using PublicServiceRegistry.PublicService.Commands;
    using Queries;
    using Requests;
    using Responses;
    using Security;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen/{id}/levensloop")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen\nLevensloop")]
    [PublicServiceRegistryAuthorize]
    public class LifeCycleController : ApiBusController
    {
        public LifeCycleController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Vraag de levensloopfases van een dienstverlening op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">Id van de dienstverlening.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de opvraging van levensloopfases gelukt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet("fases")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PublicServiceLifeCycleResponseItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PublicServiceLifeCycleResponseItemExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> ListLifeCycleStages(
            [FromServices] BackofficeContext context,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var projectionPosition = await context.GetProjectionPositionAsync(nameof(PublicServiceLifeCycleListProjections), cancellationToken);
            Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, projectionPosition.ToString());

            // idea: if dienstverleningid does not exist => 404 + documentatie aanpassen swagger

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
        /// Vraag een levensloopfase van een dienstverlening op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">Id van de dienstverlening.</param>
        /// <param name="faseId">Id van de levensloopfase.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de levensloopfase van de dienstverlening gevonden is.</response>
        /// <response code="404">Als de levensloopfase van de dienstverlening niet gevonden kan worden.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet("fases/{faseId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LifeCycleStageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LifeCycleStageResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(LifeCycleStageNotFoundResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> GetLifeCyclePhase(
            [FromServices] BackofficeContext context,
            [FromRoute] string id,
            [FromRoute] int faseId,
            CancellationToken cancellationToken = default)
        {
            var projectionPosition = await context.GetProjectionPositionAsync(nameof(PublicServiceLifeCycleListProjections), cancellationToken);
            Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, projectionPosition.ToString());

            var publicService =
                await context
                    .PublicServiceLifeCycleList
                    .AsNoTracking()
                    .SingleOrDefaultAsync(item => item.PublicServiceId == id && item.LifeCycleStageId == faseId, cancellationToken);

            if (publicService == null)
                return NotFound();

            return Ok(
                new LifeCycleStageResponse(
                    publicService.LifeCycleStageType,
                    PublicServiceRegistry.LifeCycleStageType.Parse(publicService.LifeCycleStageType).Translation.Name,
                    publicService.From,
                    publicService.To));
        }

        /// <summary>
        /// Voeg een fase toe aan de levensloop van een dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de dienstverlening.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="202">Als het verzoek aanvaard werd.</response>
        /// <response code="400">Als het verzoek ongeldige data bevat.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpPost("fases")]
        [ProducesResponseType(typeof(long), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(AddStageToLifeCycleRequest), typeof(AddStageToLifeCycleRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status202Accepted, typeof(EventStorePositionResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> AddStageToLifeCycle(
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
        /// Verander de periode van een fase in de levensloop van een dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de dienstverlening.</param>
        /// <param name="faseId">Id van de levensloopfase.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="202">Als het verzoek aanvaard werd.</response>
        /// <response code="400">Als het verzoek ongeldige data bevat.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpPut("fases/{faseId}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ChangePeriodOfLifeCycleStageRequest), typeof(ChangePeriodOfLifeCycleStageRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status202Accepted, typeof(EventStorePositionResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> ChangePeriodOfLifeCycleStage(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromRoute] int faseId,
            [FromBody] ChangePeriodOfLifeCycleStageRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(request))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    ChangePeriodOfLifeCycleStageRequestMapping.Map(id, faseId, request),
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// Verwijder een fase in de levensloop van een dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de dienstverlening.</param>
        /// <param name="faseId">Id van de levensloopfase.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="202">Als het verzoek aanvaard werd.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpDelete("fases/{faseId}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status202Accepted, typeof(EventStorePositionResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> RemoveStageFromLifeCycle(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromRoute] int faseId,
            CancellationToken cancellationToken = default)
        {
            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    new RemoveStageFromLifeCycle(new PublicServiceId(id), LifeCycleStageId.FromNumber(faseId)),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}
