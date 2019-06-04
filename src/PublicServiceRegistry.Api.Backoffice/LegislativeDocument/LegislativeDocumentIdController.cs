namespace PublicServiceRegistry.Api.Backoffice.LegislativeDocument
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests;
    using Security;
    using Swashbuckle.AspNetCore.Filters;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen/{id}/wetgevenddocument")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class LegaslitiveDocumentIdController : ApiBusController
    {
        public LegaslitiveDocumentIdController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Wijs het id van het wetgevend document toe aan een bestaande dienstverlening.
        /// </summary>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="setLegislativeDocumentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(SetLegislativeDocumentIdRequest), typeof(SetLegislativeDocumentIdRequestExample))]
        public async Task<IActionResult> Put(
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] SetLegislativeDocumentIdRequest setLegislativeDocumentId,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(setLegislativeDocumentId))
                return BadRequest(ModelState);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    SetLegislativeDocumentIdRequestMapping.Map(id, setLegislativeDocumentId),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}
