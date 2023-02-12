namespace Narcolepsy.Core.Http;

using System.Diagnostics;

internal class HttpRequestExecutor {
    private readonly HttpClient Client = new();

    public static HttpRequestExecutor Instance { get; } = new();

    public async Task<HttpResponse> Execute(HttpRequestMessage request) {
        try {
            Stopwatch Stopwatch = Stopwatch.StartNew();
            HttpResponseMessage Response = await this.Client.SendAsync(request);
            Stopwatch.Stop();
            return new HttpResponse(
                DateTime.Now,
                Stopwatch.Elapsed,
                (int)Response.StatusCode,
                Response.ReasonPhrase ?? "",
                Response.Headers.Concat(Response.Content.Headers)
                    .SelectMany(h => h.Value.Select(val => new HttpResponseHeader(h.Key, val))).ToArray(),
                await Response.Content.ReadAsByteArrayAsync(),
                null);
        } catch (Exception e) {
            return new HttpResponse(default, default, 0, "", Array.Empty<HttpResponseHeader>(), Array.Empty<byte>(),
                new RequestExecutionError(
                    "There was an error making the request",
                    e.Message,
                    e
                ));
        }
    }
}