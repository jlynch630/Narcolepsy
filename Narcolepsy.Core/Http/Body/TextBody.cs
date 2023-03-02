namespace Narcolepsy.Core.Http.Body {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TextBody : IHttpBody {
        public string BodyContent { get; set; }

        public ValueTask<Stream> GetStreamAsync() => ValueTask.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(this.BodyContent)));
    }
}
