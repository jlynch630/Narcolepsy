namespace Narcolepsy.Core.Http;

using Body;
using Narcolepsy.Platform.Logging;
using Platform.Requests;
using Platform.State;
using System.Diagnostics;

internal class HttpRequestContext : RequestContext<HttpRequestContextSnapshot>, IHttpRequestContext {
    private readonly HttpRequestExecutor Executor;
    private readonly MutableState<HttpResponse?> MutableResponse = new(null);

    public HttpRequestContext(HttpRequestContextSnapshot? snapshot) {
        this.Executor = HttpRequestExecutor.Instance;

        if (snapshot is null) return;
        this.Name.Value = snapshot.Name;
        this.Url.Value = snapshot.Url;
        this.Method.Value = snapshot.Method;
        this.Body.Value = snapshot.Body;
        foreach (HttpHeader Header in snapshot.Headers)
            this.Headers.Add(Header);
        this.MutableResponse.Value = snapshot.Response;
        this.State = snapshot.State;
    }

    public MutableState<IHttpBody> Body { get; } = new(new EmptyBody());

    public async Task Execute(CancellationToken token) {
        this.MutableResponse.Value = await this.Executor.ExecuteAsync(this, token);
    }

    public MutableListState<HttpHeader> Headers { get; } = new();

    public MutableState<string> Method { get; } = new("GET");

    public IReadOnlyState<HttpResponse?> Response => this.MutableResponse;

    public StateDictionary State { get; } = new();

    public MutableState<string> Url { get; } = new("");

    public override HttpRequestContextSnapshot Save() => new(
        this.Name.Value,
        this.Url.Value,
        this.Method.Value,
        this.Body.Value,
        this.Headers.Value.ToArray(),
        this.Response.Value,
        this.State);
}