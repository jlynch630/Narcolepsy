namespace Narcolepsy.Core.Http.Body;

using System.Text.Json.Serialization;

[JsonConverter(typeof(HttpBodyJsonConverter))]
public interface IHttpBody {
    public ValueTask<Stream> GetStreamAsync();
}