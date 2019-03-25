namespace PublicServiceRegistry.PublicService.Pipes
{
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Exceptions;

    public static class RequiresRoleExtension
    {
        public static ICommandHandlerBuilder<CommandMessage<TMessage>> RequiresRole<TMessage>(this ICommandHandlerBuilder<CommandMessage<TMessage>> source, string roleName)
            where TMessage : class
        {
            return source.Pipe(next => async (message, ct) =>
            {
                var metadata = CommandMetaData.FromDictionary(message.Metadata);
                if (metadata.HasRole(roleName))
                    return await next(message, ct);

                throw new InsufficientRights();
            });
        }
    }
}
