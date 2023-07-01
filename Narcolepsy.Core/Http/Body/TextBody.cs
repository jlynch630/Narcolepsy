namespace Narcolepsy.Core.Http.Body;

using System.Text;

public class TextBody : IHttpBody {
    public string BodyContent { get; set; } = String.Empty;

    public ValueTask<Stream> GetStreamAsync() =>
        ValueTask.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(this.BodyContent)));
}