namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.Api;

    public abstract class ApiBusController : ApiController
    {
        protected ICommandHandlerResolver Bus { get; }

        protected ApiBusController(ICommandHandlerResolver bus) => Bus = bus;

        protected IDictionary<string, object> GetMetadata()
        {
            // TODO: Move to generic part as well? This is usefull!
            if (User == null)
                return new Dictionary<string, object>();

            return new CommandMetaData(
                User,
                AddRemoteIpAddressMiddleware.UrnBasisregistersVlaanderenIp,
                AddCorrelationIdMiddleware.UrnBasisregistersVlaanderenCorrelationId)
               .ToDictionary();
        }
    }
}
