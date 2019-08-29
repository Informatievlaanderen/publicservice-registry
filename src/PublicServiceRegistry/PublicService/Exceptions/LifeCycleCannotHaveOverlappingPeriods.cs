namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class LifeCycleCannotHaveOverlappingPeriods : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "De levenscyclus van een dienstverlening mag geen overlappende periodes bevatten.";

        public LifeCycleCannotHaveOverlappingPeriods()
            : base(ExceptionMessage) { }
    }
}
