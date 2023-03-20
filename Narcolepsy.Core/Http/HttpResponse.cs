namespace Narcolepsy.Core.Http;

using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public partial record HttpResponse {
    private Lazy<string> StringValue;

    private static Regex CharsetRegex = CreateCharsetRegex();

    private static DecoderFallback DecoderReplacer = new DecoderReplacementFallback("\ufffd");

    public DateTime RequestDate { get; }

    public TimeSpan ExecutionTime { get; }

    public int StatusCode { get; }

    public string StatusText { get; }

    public HttpResponseHeader[] ResponseHeaders { get; }

    public byte[] ResponseBody { get; }

    public RequestExecutionError? Error { get; }

    [JsonIgnore]
    public HttpRequestMessage? RequestMessage { get; }

    [JsonIgnore]
    public string BodyText => this.StringValue.Value;

    public HttpResponse(DateTime requestDate,
                                   TimeSpan executionTime,
                                   HttpRequestMessage? requestMessage,
                                   int statusCode,
                                   string statusText,
                                   HttpResponseHeader[] responseHeaders,
                                   byte[] responseBody,
                                   RequestExecutionError? error) {
        this.RequestDate = requestDate;
        this.ExecutionTime = executionTime;
        this.RequestMessage = requestMessage;
        this.StatusCode = statusCode;
        this.StatusText = statusText;
        this.ResponseHeaders = responseHeaders;
        this.ResponseBody = responseBody;
        this.Error = error;
        this.StringValue = new Lazy<string>(this.GetStringBody);
    }

    public static HttpResponse CreateErrorResponse(RequestExecutionError error, HttpRequestMessage? request = null) => new HttpResponse(default, default, request, 0, "", Array.Empty<HttpResponseHeader>(), Array.Empty<byte>(),
                error);

    private string GetStringBody() => this.GetResponseEncoding().GetString(this.ResponseBody);

    private Encoding GetResponseEncoding() {
        // use the "Default" by default, haha, but otherwise use the "charset" of the content-type header
        // both ISO-8859-1 and US ASCII are claimed to be default encodings, so
        // rationale for using Default: https://www.w3.org/International/articles/http-charset/index
        //
        // syntax spec here: https://www.rfc-editor.org/rfc/rfc9110.html#media.type
        // so it seems that using a regex on charset=??? should work fine 99.9% of the time
        // technically, the sender can send a quoted-string, but they SHOULD NOT according to the spec, so we won't
        // support that yet

        // first: get the content type header, if any
        HttpResponseHeader? MatchingHeader = this.ResponseHeaders.FirstOrDefault(h => h.Name.Equals("content-type", StringComparison.OrdinalIgnoreCase));
        if (MatchingHeader is null) return Encoding.Default;

        // great, now match it's value against the regex
        Match CharsetMatch = HttpResponse.CharsetRegex.Match(MatchingHeader.Value);
        if (!CharsetMatch.Success) return Encoding.Default;

        // okay, great we have the charset. find it's matching encoding
        string Charset = CharsetMatch.Groups[1].Value;

        try {
            return Encoding.GetEncoding(Charset, EncoderFallback.ReplacementFallback, HttpResponse.DecoderReplacer);
        } catch (ArgumentException) {
            return Encoding.Default;
        }
    }

    // spec defines what a "token" is, hence the long character list
    [GeneratedRegex(";\\s*charset=(([!#$%&'*+\\-.^_`|~]|\\d|[a-z])+)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex CreateCharsetRegex();
}