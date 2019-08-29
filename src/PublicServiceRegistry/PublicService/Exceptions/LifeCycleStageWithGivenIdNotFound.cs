namespace PublicServiceRegistry.PublicService.Exceptions
{
    public class LifeCycleStageWithGivenIdNotFound : PublicServiceRegistryException
    {
        private const string ExceptionMessage = "De levensloopfase met id {0} kon niet gevonden worden.";

        public LifeCycleStageWithGivenIdNotFound(int lifeCycleStageId)
            : base(string.Format(ExceptionMessage, lifeCycleStageId)) { }
    }
}
