namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class LegislativeDocumentIdWasAlreadySet: PublicServiceRegistryException
    {
        private const string ExceptionMessage = "Aan deze dienstverlening is reeds een wetgevend document id toegewezen. " +
                                                "Gelieve eerst het vorige wetgevend document id te verwijderen vooraleer een nieuwe toe te kennen.";

        public LegislativeDocumentIdWasAlreadySet()
            : base(ExceptionMessage) { }
    }
}
