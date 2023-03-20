namespace Narcolepsy.Core.Http;

using Body;
using Narcolepsy.Platform.Requests;
using Platform.State;

internal class HttpRequestContext : RequestContext<HttpRequestContextSnapshot>, IHttpRequestContext {
    private readonly MutableState<HttpResponse?> MutableResponse = new(null);
    private readonly HttpRequestExecutor Executor;

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

    public MutableState<string> Url { get; } = new("");

    public MutableState<string> Method { get; } = new("GET");

    public MutableState<IHttpBody> Body { get; } = new(new EmptyBody());

    public MutableListState<HttpHeader> Headers { get; } = new();

    public IReadOnlyState<HttpResponse?> Response => this.MutableResponse;

	public StateDictionary State { get; } = new();

	public async Task Execute(CancellationToken token) {
        this.MutableResponse.Value = await this.Executor.ExecuteAsync(this, token);
    }

    public override HttpRequestContextSnapshot Save() => new(
        this.Name.Value,
        this.Url.Value,
        this.Method.Value,
        this.Body.Value,
        this.Headers.Value.ToArray(),
        this.Response.Value,
        this.State);
}