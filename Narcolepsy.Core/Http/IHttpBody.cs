namespace Narcolepsy.Core.Http;

public interface IHttpBody {
    public ValueTask<Stream> GetStreamAsync();
}