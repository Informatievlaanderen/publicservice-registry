namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class CannotPerformActionOnRemovedPublicService : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "Deze actie kan niet uitgevoerd worden op een reeds verwijderde dienstverlening.";

        public CannotPerformActionOnRemovedPublicService()
            : base(ExceptionMessage) { }
    }
}
