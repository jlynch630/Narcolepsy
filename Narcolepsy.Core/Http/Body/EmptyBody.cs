namespace Narcolepsy.Core.Http.Body;

internal class EmptyBody : IHttpBody {
    public ValueTask<Stream> GetStreamAsync() => ValueTask.FromResult(Stream.Null);
}