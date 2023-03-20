namespace Narcolepsy.Core.Http;

using Narcolepsy.Core.Http.Exceptions;
using System.Diagnostics;

internal class HttpRequestExecutor {
    private readonly HttpClient Client = new(new HttpClientHandler() {
        AllowAutoRedirect = false,
        AutomaticDecompression = System.Net.DecompressionMethods.All,
        UseCookies = false
    });

    public static HttpRequestExecutor Instance { get; } = new();

    public async Task<HttpResponse> ExecuteAsync(IHttpRequestContext request, CancellationToken token) {
        try {
            // validate
            this.AssertHeadersValid(request);

            // create the request
            HttpRequestMessage Message = await this.BuildRequestMessageAsync(request);

            // and execute
            return await this.ExecuteMessageAsync(Message, token);
        } catch (Exception e) {
            return HttpResponse.CreateErrorResponse(
                new RequestExecutionError(
                    null,
                    null,
                    e
                ));
        }
    }

    private async Task<HttpRequestMessage> BuildRequestMessageAsync(IHttpRequestContext request) {
        HttpContent Content = new StreamContent(await request.Body.Value.GetStreamAsync());

        // do content headers first
        foreach (HttpHeader Header in request.Headers.Value.Where(
                     h => h.IsEnabled && h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase)))
            Content.Headers.Add(Header.Name, Header.Value);

        // then other headers
        HttpRequestMessage Message = new() {
            Method = new HttpMethod(request.Method.Value),
            RequestUri = new Uri(request.Url.Value),
            Content = Content
        };

        foreach (HttpHeader Header in request.Headers.Value.Where(
                     h => h.IsEnabled && !h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase)))
            Message.Headers.Add(Header.Name, Header.Value);

        return Message;
    }

    private async Task<HttpResponse> ExecuteMessageAsync(HttpRequestMessage request, CancellationToken token) {
        try {
            Stopwatch Stopwatch = Stopwatch.StartNew();
            HttpResponseMessage Response = await this.Client.SendAsync(request, token);
            Stopwatch.Stop();
            return new HttpResponse(
                DateTime.Now,
                Stopwatch.Elapsed,
                request,
                (int)Response.StatusCode,
                Response.ReasonPhrase ?? "",
                Response.Headers.Concat(Response.Content.Headers)
                    .SelectMany(h => h.Value.Select(val => new HttpResponseHeader(h.Key, val))).ToArray(),
                await Response.Content.ReadAsByteArrayAsync(),
                null);
        } catch (Exception e) {
            return HttpResponse.CreateErrorResponse(
                new RequestExecutionError(
                    null,
                    null,
                    e
                ));
        }
    }

    private void AssertHeadersValid(IHttpRequestContext request) {
        HttpHeader[] InvalidHeaders = request.Headers.Value.Where(h => !h.IsValueValid || !h.IsNameValid).ToArray();
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