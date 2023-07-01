namespace Narcolepsy.Core.Http;

using Body;
using Platform.State;

internal record HttpRequestContextSnapshot(
    string Name,
    string Url,
    string Method,
    IHttpBody Body,
    HttpHeader[] Headers,
    HttpResponse? Response,
    StateDictionary State);