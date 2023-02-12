namespace Narcolepsy.Core.Http;

public interface IHttpBody {
    public Task<Stream> GetStreamAsync();
}