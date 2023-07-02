namespace Narcolepsy.App.Services {
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class NarcolepsyLoggerProvider : ILoggerProvider {
        private readonly ILogger Logger = new NarcolepsyLogger();
        public void Dispose() {
        }

        public ILogger CreateLogger(string categoryName) => this.Logger;
    }
}
