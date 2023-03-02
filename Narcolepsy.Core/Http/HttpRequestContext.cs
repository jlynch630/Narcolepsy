namespace Narcolepsy.Core.Http;

using Body;
using Narcolepsy.Core.Http.Exceptions;
using Narcolepsy.Platform.Requests;
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

    public async Task Execute(CancellationToken token) {
        try {
            HttpContent Content = new StreamContent(await this.Body.Value.GetStreamAsync());

            this.AssertHeadersValid();

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

            this.MutableResponse.Value = await this.Executor.Execute(Message, token);
        } catch (Exception e) {
            this.MutableResponse.Value = HttpResponse.CreateErrorResponse(
                new RequestExecutionError(
                    null,
                    null,
                    e
                ));
        }
    }

    private void AssertHeadersValid() {
        HttpHeader[] InvalidHeaders = this.Headers.Value.Where(h => !h.IsValueValid || !h.IsNameValid).ToArray();
        if (!InvalidHeaders.Any()) return;

        string[] InvalidNames = InvalidHeaders.Where(h => !h.IsNameValid).Select(h => $"\"{h.Name}\"").ToArray();
        string[] InvalidValues = InvalidHeaders.Where(h => !h.IsValueValid).Select(h => $"\"{h.Name}\"").ToArray();
        string NameError = InvalidNames.Length switch {
            0 => "",
            1 => $"Header {InvalidNames[0]} has an invalid name.",
            _ => $"Headers {String.Join(", ", InvalidNames)} have invalid names.",
        };
        string ValueError = InvalidValues.Length switch {
            0 => "",
            1 => $"Header {InvalidValues[0]} has an invalid value.",
            _ => $"Headers {String.Join(", ", InvalidValues)} have invalid values.",
        };
        throw new InvalidRequestException(InvalidRequestType.Headers, (NameError + " " + ValueError).Trim());
    }
}