namespace Narcolepsy.Core.Http {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record RequestExecutionError(string Message, string HelpText, Exception? Exception);
}
