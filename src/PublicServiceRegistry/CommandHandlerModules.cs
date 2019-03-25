namespace PublicServiceRegistry
{
    using Autofac;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using PublicService;

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<PublicServiceCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(PublicServiceCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
        }
    }
}
