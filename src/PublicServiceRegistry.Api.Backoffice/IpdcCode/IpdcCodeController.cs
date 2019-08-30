namespace PublicServiceRegistry.Api.Backoffice.IpdcCode
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Projections.Backoffice;
    using PublicService;
    using Requests;
    using Security;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("dienstverleningen/{id}/ipdccode")]
    [ApiExplorerSettings(GroupName = "Dienstverleningen")]
    [PublicServiceRegistryAuthorize]
    public class IpdcCodeController : ApiBusController
    {
        public IpdcCodeController(ICommandHandlerResolver bus) : base(bus) { }

        /// <summary>
        /// Wijs de ipdc code toe aan een bestaande dienstverlening.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandId">Unieke id voor het verzoek.</param>
        /// <param name="id">Id van de bestaande dienstverlening.</param>
        /// <param name="setIpdcCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(SetIpdcCodeRequest), typeof(SetIpdcCodeRequestExample))]
        public async Task<IActionResult> Put(
            [FromServices] BackofficeContext context,
            [FromCommandId] Guid commandId,
            [FromRoute] string id,
            [FromBody] SetIpdcCodeRequest setIpdcCode,
            CancellationToken cancellationToken = default)
        {
            if (!TryValidateModel(setIpdcCode))
                return BadRequest(ModelState);

            await context.CheckPublicServiceAsync(id, cancellationToken);

            return Accepted(
                await Bus.Dispatch(
                    commandId,
                    SetIpdcCodeRequestMapping.Map(id, setIpdcCode),
                    GetMetadata(),
                    cancellationToken));
        }
    }
}
