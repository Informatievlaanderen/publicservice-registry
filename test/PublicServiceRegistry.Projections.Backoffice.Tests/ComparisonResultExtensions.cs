namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System.Text;
    using KellermanSoftware.CompareNetObjects;

    public static class ComparisonResultExtensions
    {
        public static string CreateDifferenceMessage(this ComparisonResult result, object[] actual, object[] expected)
        {
            var message = new StringBuilder();

            message
                .AppendTitleBlock("Expected", expected, Formatters.NamedJsonMessage)
                .AppendTitleBlock("But", actual, Formatters.NamedJsonMessage)
                .AppendTitleBlock("Difference", result.DifferencesString.Trim());

            return message.ToString();
        }
    }
}
