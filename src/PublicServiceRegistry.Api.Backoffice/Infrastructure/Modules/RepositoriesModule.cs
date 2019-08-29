namespace PublicServiceRegistry.Api.Backoffice.Infrastructure.Modules
{
    using Autofac;
    using PublicService;
    using PublicServiceRegistry.PublicService;

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
            => containerBuilder
                .RegisterType<PublicServices>()
                .As<IPublicServices>();
    }
}
