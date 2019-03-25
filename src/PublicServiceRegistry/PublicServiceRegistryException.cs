namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public abstract class PublicServiceRegistryException : DomainException
    {
        protected PublicServiceRegistryException() { }

        protected PublicServiceRegistryException(string message) : base(message) { }

        protected PublicServiceRegistryException(string message, Exception inner) : base(message, inner) { }
    }
}
