namespace Narcolepsy.App.Services {
    using Microsoft.Extensions.Logging;
    using Narcolepsy.Platform.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class NarcolepsyLogger : ILogger {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            string MessageText = formatter(state, exception);
            switch (logLevel) {
                case LogLevel.Trace:
                    Logger.Verbose(MessageText);
                    break;
                case LogLevel.Debug:
                    Logger.Debug(MessageText);
                    break;
                case LogLevel.Information:
                    Logger.Information(MessageText);
                    break;
                case LogLevel.Warning:
                    Logger.Warning(MessageText);
                    break;
                case LogLevel.Error:
                    Logger.Error(MessageText);
                    break;
                case LogLevel.Critical:
                    Logger.Error(MessageText);
                    break;
                case LogLevel.None:
                    Logger.Verbose(MessageText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => new Scope();

        private class Scope : IDisposable {
            public void Dispose() {
                
            }
        }
    }
}
