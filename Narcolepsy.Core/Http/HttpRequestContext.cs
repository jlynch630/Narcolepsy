namespace Narcolepsy.Core.Http;

using Body;
using Platform.State;

internal class HttpRequestContext : IHttpRequestContext {
    private readonly MutableState<HttpResponse?> MutableResponse = new(null);
    private readonly HttpRequestExecutor Executor;

    public HttpRequestContext() => this.Executor = HttpRequestExecutor.Instance;

    public MutableState<string> Name { get; } = new("");

    public MutableState<string> Url { get; } = new("");

    public MutableState<string> Method { get; } = new("GET");

    public MutableState<IHttpBody> Body { get; } = new(new EmptyBody());

    public MutableListState<HttpHeader> Headers { get; } = new();

    public IReadOnlyState<HttpResponse?> Response => this.MutableResponse;

    public async Task Execute() {
        HttpContent Content = new StreamContent(await this.Body.Value.GetStreamAsync());

        // do content headers first
        foreach (HttpHeader Header in this.Headers.Value.Where(
                     h => h.IsEnabled && h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase)))
            Content.Headers.Add(Header.Name, Header.Value);

        // then other headers
        HttpRequestMessage Message = new() {
            Method = new HttpMethod(this.Method.Value),
            RequestUri = new Uri(this.Url.Value),
            Content = Content
        };

        foreach (HttpHeader Header in this.Headers.Value.Where(
                     h => h.IsEnabled && !h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase)))
            Message.Headers.Add(Header.Name, Header.Value);

        this.MutableResponse.Value = await this.Executor.Execute(Message);
    }
}