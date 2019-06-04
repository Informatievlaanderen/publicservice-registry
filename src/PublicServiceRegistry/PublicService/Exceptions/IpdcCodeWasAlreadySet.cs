namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class IpdcCodeWasAlreadySet: PublicServiceRegistryException
    {
        private const string ExceptionMessage = "Aan deze dienstverlening is reeds een ipdc code toegewezen. " +
                                                "Gelieve eerst de vorige ipdc code te verwijderen vooraleer een nieuwe toe te kennen.";

        public IpdcCodeWasAlreadySet()
            : base(ExceptionMessage) { }
    }
}
