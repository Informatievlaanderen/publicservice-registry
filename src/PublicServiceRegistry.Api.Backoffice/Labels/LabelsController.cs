namespace PublicServiceRegistry.Api.Backoffice.Labels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.ETag;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Converters;
    using Projections.Backoffice;
    using Requests;
    using Responses;
    using Security;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen/{id}/alternatievebenamingen")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class LabelsController : ApiBusController
    {
        public LabelsController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Vraag een lijst met dienstverleningen op.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Als de opvraging van een lijst met dienstverlening gelukt is.</response>
        /// <response code="400">Als de gevraagde status een ongeldige waarde is.</response>
        /// <response code="412">Als de gevraagde minimum positie van de event store nog niet bereikt is.</response>
        /// <response code="500">Als er een interne fout is opgetreden.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<LabelListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PreconditionFailedResult), StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LabelListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status412PreconditionFailed, typeof(PreconditionFailedResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> List(
            [FromServices] BackofficeContext context,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            return Ok(await context
                .PublicServiceLabelList
                .Where(item => item.PublicServiceId == id)
                .ToListAsync(cancellationToken));
        }

        /// <summary>
        /// Wijzig een bestaande dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="updateLabelsRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(UpdateLabelsRequest), typeof(UpdateLabelsRequestExample))]
        public async Task<IActionResult> Put(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] UpdateLabelsRequest updateLabelsRequest,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(updateLabelsRequest))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    UpdateLabelsRequestMapping.Map(id, updateLabelsRequest),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}
