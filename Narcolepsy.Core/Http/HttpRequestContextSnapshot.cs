namespace Narcolepsy.Core.Http
{
    using Narcolepsy.Core.Http.Body;
    using Narcolepsy.Platform.State;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal record HttpRequestContextSnapshot(
        string Name,
        string Url,
        string Method,
        IHttpBody Body,
        HttpHeader[] Headers,
        HttpResponse? Response,
        StateDictionary State);
}
