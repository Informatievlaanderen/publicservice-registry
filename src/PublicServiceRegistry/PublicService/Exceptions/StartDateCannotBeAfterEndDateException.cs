namespace PublicServiceRegistry.PublicService.Exceptions
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public class StartDateCannotBeAfterEndDateException : DomainException
    {
        public StartDateCannotBeAfterEndDateException() : base("De startdatum mag niet na de einddatum vallen.")
        {
        }
    }
}
