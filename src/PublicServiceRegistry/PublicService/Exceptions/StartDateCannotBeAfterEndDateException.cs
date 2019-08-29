namespace PublicServiceRegistry.PublicService.Exceptions
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public class StartDateCannotBeAfterEndDateException : DomainException
    {
        private const string ExceptionMessage = "De startdatum mag niet na de einddatum vallen.";

        public StartDateCannotBeAfterEndDateException()
            : base(ExceptionMessage) { }
    }
}
