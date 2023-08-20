namespace Narcolepsy.Core.Http;

using System.Diagnostics;
using System.Net;
using System.Reflection.PortableExecutable;
using Exceptions;
using Narcolepsy.Platform.Logging;

internal class HttpRequestExecutor {
    private readonly HttpClient Client = new(new HttpClientHandler {
                                                                       AllowAutoRedirect = false,
                                                                       AutomaticDecompression =
                                                                           DecompressionMethods.All,
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
            HttpResponse Response = await this.ExecuteMessageAsync(Message, token);
            
            // dispose the message
            (Message.Content as StreamContent)?.Dispose();

            return Response;
        }
        catch (Exception e) {
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
            _ => $"Headers {String.Join(", ", InvalidNames)} have invalid names."
        };
        string ValueError = InvalidValues.Length switch {
            0 => "",
            1 => $"Header {InvalidValues[0]} has an invalid value.",
            _ => $"Headers {String.Join(", ", InvalidValues)} have invalid values."
        };
        throw new InvalidRequestException(InvalidRequestType.Headers, (NameError + " " + ValueError).Trim());
    }

    private async Task<HttpRequestMessage> BuildRequestMessageAsync(IHttpRequestContext request) {
        Logger.Debug("Building request from body {BodyType}", request.Body.Value.GetType().Name);
        MemoryStream ContentStream = new();
        await request.Body.Value.WriteAsync(ContentStream);
        ContentStream.Seek(0, SeekOrigin.Begin);
        HttpContent Content = new StreamContent(ContentStream);

        // do content headers first
        foreach (HttpHeader Header in request.Headers.Value.Where(
                     h => h.IsEnabled && h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))) {
            Content.Headers.TryAddWithoutValidation(Header.Name, Header.Value);
            Logger.Verbose("Adding content header {HeaderName}", Header.Name);

        }

        // then other headers
        HttpRequestMessage Message = new() {
                                               Method = new HttpMethod(request.Method.Value),
                                               RequestUri = new Uri(request.Url.Value),
                                               Content = Content
                                           };

        foreach (HttpHeader Header in request.Headers.Value.Where(
                     h => h.IsEnabled && !h.Name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))) {
            Message.Headers.TryAddWithoutValidation(Header.Name, Header.Value);
            Logger.Verbose("Adding header {HeaderName}", Header.Name);
        }

        Logger.Debug("Built request {Request}", Message);
        return Message;
    }

    private async Task<HttpResponse> ExecuteMessageAsync(HttpRequestMessage request, CancellationToken token) {
        try {
            Stopwatch Stopwatch = Stopwatch.StartNew();
            HttpResponseMessage Response = await this.Client.SendAsync(request, token);
            Logger.Debug("Received headers in {Duration}ms", Stopwatch.ElapsedMilliseconds);
            byte[] Content = await Response.Content.ReadAsByteArrayAsync(token);
            Stopwatch.Stop();
            Logger.Debug("Successfully executed request in {Duration}ms", Stopwatch.ElapsedMilliseconds);

            return new HttpResponse(
                DateTime.Now,
                Stopwatch.Elapsed,
                request,
                (int)Response.StatusCode,
                Response.ReasonPhrase ?? "",
                Response.Headers.Concat(Response.Content.Headers)
                        .SelectMany(h => h.Value.Select(val => new HttpResponseHeader(h.Key, val))).ToArray(),
                Content,
                null);
        }
        catch (Exception e) {
            Logger.Debug(e, "Request failed");

            return HttpResponse.CreateErrorResponse(
                new RequestExecutionError(
                    null,
                    null,
                    e
                ));
        }
    }
}