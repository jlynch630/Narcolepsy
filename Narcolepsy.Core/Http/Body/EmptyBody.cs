namespace Narcolepsy.Core.Http.Body {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class EmptyBody : IHttpBody {
        public Task<Stream> GetStreamAsync() => Task.FromResult(Stream.Null);
    }
}
