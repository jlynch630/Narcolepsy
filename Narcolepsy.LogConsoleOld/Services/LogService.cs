using System;

namespace Narcolepsy.LogConsole.Services {
    using Narcolepsy.LogConsole.Data;
    using System;
    using System.Collections.Generic;
    using System.IO.Pipes;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    internal class LogService {
        private PipeStream Client;

        public event EventHandler<LogEntry> LogEntryAvailable;

        private CancellationTokenSource ReadToken;

        public void Connect() {
            if (this.ReadToken != null) return;
            string[] Args = Environment.GetCommandLineArgs();
            string Command = Environment.CommandLine;
            if (Args.Length < 2) throw new ApplicationException("Unable to connect, pipe handle was null");
            string PipeName = Args[1];
            this.Client = new AnonymousPipeClientStream(PipeDirection.In, PipeName);
            this.ReadToken = new CancellationTokenSource();
            Task.Run(this.ListenForMessages);
        }

        public void Stop() {
            this.ReadToken?.Cancel();
        }

        private async Task ListenForMessages() {
            byte[] Buffer = new byte[2048];
            StringBuilder Current = new();
            while (!this.ReadToken.IsCancellationRequested) {
                // read into the buffer
                int BytesRead = await this.Client.ReadAsync(Buffer, this.ReadToken.Token);
                int TerminatorIndex = 0;
                int StartIndex = 0;

                // very simple null terminated messages
                while (TerminatorIndex != -1) {
                    TerminatorIndex = Array.IndexOf(Buffer, 0, StartIndex, BytesRead - StartIndex);
                    int EndIndex = TerminatorIndex == -1 ? BytesRead : TerminatorIndex;
                    string Data = Encoding.UTF8.GetString(Buffer[StartIndex..EndIndex]);
                    Current.Append(Data);

                    if (TerminatorIndex != -1) {
                        // we've finished reading data! process it
                        this.ProcessLogMessage(Current.ToString());
                        Current.Clear();
                    }
                    StartIndex = TerminatorIndex + 1;
                }
            }
            this.ReadToken = null;
        }

        private void ProcessLogMessage(string message) {
            LogEntry Entry = JsonSerializer.Deserialize<LogEntry>(message);
            this.LogEntryAvailable?.Invoke(this, Entry);
        }
    }
}
