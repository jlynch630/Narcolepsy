namespace Narcolepsy.Core.Http.Body
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class EmptyBody : IHttpBody {
        public ValueTask<Stream> GetStreamAsync() => ValueTask.FromResult(Stream.Null);
    }
}
