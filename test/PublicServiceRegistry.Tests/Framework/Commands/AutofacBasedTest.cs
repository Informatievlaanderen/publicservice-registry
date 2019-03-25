namespace PublicServiceRegistry.Tests.PublicServiceCommands
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Comparers;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Api.Backoffice.Infrastructure.Modules;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit.Abstractions;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    public class CustomResolver : IHandlerResolver
    {
        private readonly ICommandHandlerResolver _commandHandlerResolver;

        public CustomResolver(ICommandHandlerResolver commandHandlerResolver)
            => _commandHandlerResolver = commandHandlerResolver ?? throw new ArgumentNullException(nameof(commandHandlerResolver));

        public Func<object, Task<long>> ResolveHandlerFor(object command)
        {
            var typeOfCommandMessage = command.GetType();
            if (typeOfCommandMessage.IsGenericType && typeOfCommandMessage.GetGenericTypeDefinition() == typeof(CommandMessage<>))
            {
                var typeOfCommand = typeOfCommandMessage.GenericTypeArguments[0];
                var resolveHandlerMethod = typeof(ICommandHandlerResolver).GetRuntimeMethod("Resolve", new Type[0]).MakeGenericMethod(typeOfCommand);
                var handler = resolveHandlerMethod.Invoke(_commandHandlerResolver, new object[0]);
                //var commandMessageType = typeof(CommandMessage<>).MakeGenericType(command.GetType());
                var invokeHandler = handler.GetType().GetRuntimeMethod("Invoke", new[] { typeOfCommandMessage, typeof(CancellationToken) });
                //var commandMessageConstructor = commandMessageType.GetConstructor(new[] { typeof(Guid), command.GetType(), typeof(IDictionary<string, object>) });
                //object CommandMessageFactory(object cmd) => commandMessageConstructor.Invoke(new[] { Guid.NewGuid(), cmd, new Dictionary<string, object> { { "MessagePurpose", "Testing" } } });
                return async cmd => await (Task<long>)invokeHandler.Invoke(handler, new[] { cmd, CancellationToken.None });
            }
            else
            {
                //a lot of reflection here, perhaps CommandHandling library could offer a less type dependent way of handling commands...?
                var resolveHandlerMethod = typeof(ICommandHandlerResolver).GetRuntimeMethod("Resolve", new Type[0]).MakeGenericMethod(command.GetType());
                var handler = resolveHandlerMethod.Invoke(_commandHandlerResolver, new object[0]);
                var commandMessageType = typeof(CommandMessage<>).MakeGenericType(command.GetType());
                var invokeHandler = handler.GetType().GetRuntimeMethod("Invoke", new[] { commandMessageType, typeof(CancellationToken) });
                var commandMessageConstructor = commandMessageType.GetConstructor(new[] { typeof(Guid), command.GetType(), typeof(IDictionary<string, object>) });
                object CommandMessageFactory(object cmd) => commandMessageConstructor.Invoke(new[] { Guid.NewGuid(), cmd, new Dictionary<string, object> { { "MessagePurpose", "Testing" } } });
                return async cmd => await (Task<long>)invokeHandler.Invoke(handler, new[] { CommandMessageFactory(cmd), CancellationToken.None });
            }
        }
    }

    public class AutofacBasedTest
    {
        private IContainer _container;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IList<Action<ContainerBuilder>> _containerModifications;

        protected IExceptionCentricTestSpecificationRunner ExceptionCentricTestSpecificationRunner => _container.Resolve<IExceptionCentricTestSpecificationRunner>();

        protected IEventCentricTestSpecificationRunner EventCentricTestSpecificationRunner => _container.Resolve<IEventCentricTestSpecificationRunner>();

        protected IFactComparer FactComparer => _container.Resolve<IFactComparer>();

        protected IExceptionComparer ExceptionComparer => _container.Resolve<IExceptionComparer>();

        protected ILogger Logger => _container.Resolve<ILogger>();

        public AutofacBasedTest(ITestOutputHelper testOutputHelper)
        {
            _containerModifications = new List<Action<ContainerBuilder>>();
            _testOutputHelper = testOutputHelper;

            var containerBuilder = CreateDefaultContainer(testOutputHelper);

            _container = containerBuilder.Build();
        }

        public virtual void ModifyContainer(Action<ContainerBuilder> builder)
        {
            _containerModifications.Add(builder);

            var containerBuilder = CreateDefaultContainer(_testOutputHelper);

            foreach (var containerModification in _containerModifications)
            {
                containerModification(containerBuilder);
            }

            _container = containerBuilder.Build();
        }

        private ContainerBuilder CreateDefaultContainer(ITestOutputHelper testOutputHelper)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "ConnectionStrings:Events", "x" } })
                .Build();

            var eventSerializerSettings = EventsJsonSerializerSettingsProvider.CreateSerializerSettings();

            var containerBuilder = new ContainerBuilder();

            var services = new ServiceCollection();
            containerBuilder
                .RegisterModule(new TestLoggingModule(services))
                .RegisterModule(new EventHandlingModule(typeof(DomainAssemblyMarker).Assembly, eventSerializerSettings))
                .RegisterModule(new CommandHandlingModule(configuration))
                .RegisterModule(new SqlStreamStoreModule());

            containerBuilder
                .RegisterType<CustomResolver>()
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterType<StreamStoreFactRepository>()
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterInstance(CreateFactComparer())
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterInstance(CreateExceptionComparer())
                .AsImplementedInterfaces();


            containerBuilder
                .RegisterType<ExceptionCentricTestSpecificationRunner>()
                .AsImplementedInterfaces();

            containerBuilder.
                RegisterType<EventCentricTestSpecificationRunner>()
                .AsImplementedInterfaces();

            containerBuilder.RegisterInstance(testOutputHelper);
            containerBuilder.RegisterType<XUnitLogger>().AsImplementedInterfaces();

            containerBuilder.Populate(services);

            return containerBuilder;
        }

        protected virtual IFactComparer CreateFactComparer()
            => new CompareNetObjectsBasedFactComparer(new CompareLogic());

        protected virtual IExceptionComparer CreateExceptionComparer()
        {
            var comparer = new CompareLogic();
            comparer.Config.MembersToIgnore.Add("Source");
            comparer.Config.MembersToIgnore.Add("StackTrace");
            comparer.Config.MembersToIgnore.Add("Message");
            comparer.Config.MembersToIgnore.Add("TargetSite");
            return new CompareNetObjectsBasedExceptionComparer(comparer);
        }

        protected void Assert(IExceptionCentricTestSpecificationBuilder builder)
            => builder.Assert(ExceptionCentricTestSpecificationRunner, ExceptionComparer, Logger);

        protected void Assert(IEventCentricTestSpecificationBuilder builder)
            => builder.Assert(EventCentricTestSpecificationRunner, FactComparer, Logger);
    }
}
