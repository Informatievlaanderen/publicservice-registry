namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class InsufficientRights : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "U beschikt niet over de juiste rechten om deze actie uit te voeren.";

        public InsufficientRights()
            : base(ExceptionMessage) { }
    }

    public class IpdcCodeWasAlreadySet: PublicServiceRegistryException
    {
        private const string ExceptionMessage = "Op deze dienstverlening is reeds een ipdc code toegewezen. " +
                                                "Gelieve eerst de vorige ipdc code te verwijderen vooraleer een nieuwe toe te kennen.";

        public IpdcCodeWasAlreadySet()
            : base(ExceptionMessage) { }
    }
}
