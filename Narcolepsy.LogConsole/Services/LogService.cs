namespace Narcolepsy.LogConsole.Services {
    using System;
    using Data;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Parsing;

    public class LogService : ILogEventSink {
        private MessageTemplateParser TemplateParser = new();
        public event EventHandler<LogEntry> LogEntryAvailable;

        public void RecordLogMessage(LogEntry entry) => this.LogEntryAvailable?.Invoke(this, entry);

        public void Emit(LogEvent logEvent) {
            // convert the log event to a log entry
            // we have: time, level, source, message w/ properties
            LogToken Space = new(" ", LogTokenStyle.None);
            LogToken Time = new($"[{logEvent.Timestamp:HH:mm:ss}]", new LogTokenStyle("#4DB6AC", null, false, false, false));
            string DefaultColor = logEvent.Level switch {
                LogEventLevel.Information => "white",
                LogEventLevel.Warning => "#ffc107",
                LogEventLevel.Error => "#ef5350",
                _ => null
            };
            LogTokenStyle DefaultStyle = new LogTokenStyle(DefaultColor, null, false, false, false);
            LogToken Level = new(logEvent.Level switch {
                LogEventLevel.Information => "INFO".PadLeft(7),
                _ => logEvent.Level.ToString().ToUpper().PadLeft(7),
            }, DefaultStyle with {Bold = true});

            string SourceText = logEvent.Properties["Assembly"].ToString()[1..^1];
            LogToken Source = new("(" + SourceText + ")", new LogTokenStyle(SourceText == "Narcolepsy.App" ? "#607D8B" : "#BA68C8", null, false, false, true));
            MessageTemplate NewTemplate = this.TemplateParser.Parse(logEvent.MessageTemplate.Text.Replace("[{Assembly}]", "").Trim());
            IEnumerable<LogToken> MessageTokens = NewTemplate.Tokens.Select(t => {
                if (t is TextToken Text) return new LogToken(Text.Text, DefaultStyle);

                PropertyToken Property = t as PropertyToken;
                StringWriter StringWriter = new();
                LogEventPropertyValue PropertyValue =
                    logEvent.Properties.GetValueOrDefault(Property.PropertyName, null);
                if (PropertyValue is ScalarValue Scalar)
                    StringWriter.Write(Scalar.Value?.ToString() ?? "null");
                else
                    Property.Render(logEvent.Properties, StringWriter);
                return new LogToken(StringWriter.ToString(), DefaultStyle with { Foreground = "#4DD0E1" });
            });

            LogToken[] Tokens = new[] { Time, Space, Level, Space, Source, Space }.Concat(MessageTokens).ToArray();
            this.RecordLogMessage(new LogEntry(Tokens));
        }
    }
}
