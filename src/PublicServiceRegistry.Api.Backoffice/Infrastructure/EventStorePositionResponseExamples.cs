namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using Swashbuckle.AspNetCore.Filters;

    public class EventStorePositionResponseExamples : IExamplesProvider
    {
        public object GetExamples() => 42;
    }
}
