namespace PublicServiceRegistry.Api.Backoffice.Tests
{
    using System;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        public ILogger CreateLogger(string categoryName)
            => new XunitLogger(_testOutputHelper, categoryName);

        public void Dispose() { }
    }

    public class XunitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;

        public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = $"{_categoryName} [{eventId}] {formatter(state, exception)}";
            _testOutputHelper.WriteLine(message);
            Console.WriteLine(message);

            if (exception == null)
                return;

            var exceptionMessage = exception.ToString();
            _testOutputHelper.WriteLine(exceptionMessage);
            Console.WriteLine(exceptionMessage);
        }

        private class NoopDisposable : IDisposable
        {
            public static readonly NoopDisposable Instance = new NoopDisposable();

            public void Dispose() { }
        }
    }
}
