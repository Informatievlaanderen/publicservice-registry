namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class InsufficientRights : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "U beschikt niet over de juiste rechten om deze actie uit te voeren.";

        public InsufficientRights()
            : base(ExceptionMessage) { }
    }
}
