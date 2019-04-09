namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using Newtonsoft.Json;

    public static class Formatters
    {
        public static string NamedJsonMessage<T>(T message) => $"{message.GetType().Name} - {JsonConvert.SerializeObject(message, Formatting.Indented)}";
    }
}
