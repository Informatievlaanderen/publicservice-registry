namespace PublicServiceRegistry.Api.Backoffice.Infrastructure
{
    using Swashbuckle.AspNetCore.Filters;

    public class EventStorePositionResponseExamples : IExamplesProvider<int>
    {
        public int GetExamples() => 42;
    }
}
