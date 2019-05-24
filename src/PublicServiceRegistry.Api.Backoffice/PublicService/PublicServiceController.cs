namespace PublicServiceRegistry.Api.Backoffice.PublicService
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
    using Infrastructure.ETag;
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
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class PublicServiceController : ApiBusController
    {
        public PublicServiceController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Vraag een dienstverlening op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">Identificator van de dienstverlening.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de dienstverlening gevonden is.</response>
        /// <response code="404">Als de dienstverlening niet gevonden kan worden.</response>
        /// <response code="412">Als de gevraagde minimum positie van de event store nog niet bereikt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PublicServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PublicServiceResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(PublicServiceNotFoundResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status412PreconditionFailed, typeof(PreconditionFailedResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            [FromServices] BackofficeContext context,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var projectionPosition = await context.GetProjectionPositionAsync(cancellationToken);
            Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, projectionPosition.ToString());

            var publicService =
                await context
                    .PublicServiceList
                    .AsNoTracking()
                    .SingleOrDefaultAsync(item => item.PublicServiceId == id, cancellationToken);

            if (publicService == null)
                return NotFound();

            return Ok(
                new PublicServiceResponse(
                    publicService.PublicServiceId,
                    publicService.Name,
                    publicService.CompetentAuthorityCode,
                    publicService.CompetentAuthorityName,
                    publicService.ExportToOrafin,
                    publicService.CurrentLifeCycleStageType,
                    !string.IsNullOrEmpty(publicService.CurrentLifeCycleStageType)
                        ? PublicServiceRegistry.LifeCycleStageType.Parse(publicService.CurrentLifeCycleStageType).Translation.Name
                        : string.Empty,
                    publicService.IpdcCode));
        }

        /// <summary>
        /// Vraag een lijst met dienstverleningen op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de opvraging van een lijst met dienstverlening gelukt is.</response>
        /// <response code="400">Als de gevraagde status een ongeldige waarde is.</response>
        /// <response code="412">Als de gevraagde minimum positie van de event store nog niet bereikt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PublicServiceListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PreconditionFailedResult), StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PublicServiceListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status412PreconditionFailed, typeof(PreconditionFailedResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> List(
            [FromServices] BackofficeContext context,
            CancellationToken cancellationToken = default)
        {
            var filtering = Request.ExtractFilteringRequest<PublicServiceListItemFilter>();
            var sorting = Request.ExtractSortingRequest();
            var pagination = Request.ExtractPaginationRequest();

            var pagedOrganisations =
                new PublicServiceListQuery(context)
                    .Fetch(filtering, sorting, pagination);

            Response.AddPagedQueryResultHeaders(pagedOrganisations);

            var projectionPosition = await context.GetProjectionPositionAsync(cancellationToken);
            Response.Headers.Add(PublicServiceHeaderNames.LastObservedPosition, projectionPosition.ToString());

            return Ok(await pagedOrganisations.Items.ToListAsync(cancellationToken));
        }

        /// <summary>
        /// Maak een nieuwe dienstverlening aan.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dvrCodeGenerator"></param>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="registerPublicService"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(RegisterPublicServiceRequest), typeof(RegisterPublicServiceRequestExample))]
        public async Task<IActionResult> Post(
            [FromServices] BackofficeContext context,
            [FromServices] DvrCodeGenerator dvrCodeGenerator,
            [FromCommandId] Guid commandId,
            [FromBody] RegisterPublicServiceRequest registerPublicService,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publicServiceId = await dvrCodeGenerator.GenerateDvrCode();

            return Accepted(
                $"/v1/dienstverleningen/{publicServiceId}",
                await Bus.Dispatch(
                    commandId,
                    RegisterPublicServiceRequestMapping.Map(registerPublicService, publicServiceId),
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// Wijzig een bestaande dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="updatePublicService"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(UpdatePublicServiceRequest), typeof(UpdatePublicServiceRequestExample))]
        public async Task<IActionResult> Put(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] UpdatePublicServiceRequest updatePublicService,
            CancellationToken cancellationToken = default)
        {
            var internalMessage = new UpdatePublicServiceInternalRequest(id, updatePublicService);

            if (!TryValidateModel(internalMessage))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    UpdatePublicServiceRequestMapping.Map(internalMessage),
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// Verwijder een bestaande dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="removePublicServiceRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(RemovePublicServiceRequest), typeof(RemovePublicServiceRequestExample))]
        public async Task<IActionResult> Delete(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] RemovePublicServiceRequest removePublicServiceRequest,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(removePublicServiceRequest))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    RemovePublicServiceRequestMapping.Map(id, removePublicServiceRequest),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}
