namespace Narcolepsy.Core.Http;

using Body;
using Platform.Requests;
using Platform.State;

public interface IHttpRequestContext : IRequestContext {
    public MutableState<IHttpBody> Body { get; }

    public MutableListState<HttpHeader> Headers { get; }

    public MutableState<string> Method { get; }

    public IReadOnlyState<HttpResponse?> Response { get; }

    public StateDictionary State { get; }

    public MutableState<string> Url { get; }

    public Task Execute(CancellationToken token);
}