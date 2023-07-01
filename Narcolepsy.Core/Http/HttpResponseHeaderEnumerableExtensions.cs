namespace Narcolepsy.Core.Http;

public static class HttpResponseHeaderEnumerableExtensions {
    // todo: O(n)ness vs O(1) ness for dictionaries. who's to say, but will keep in mind.

    public static HttpResponseHeader[] GetHeaders(this IEnumerable<HttpResponseHeader> headers, string name) {
        return headers.Where(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();
    }

    public static string? GetHeaderValue(this IEnumerable<HttpResponseHeader> headers, string name) =>
        headers.FirstOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value;

    public static string[] GetHeaderValues(this IEnumerable<HttpResponseHeader> headers, string name) =>
        headers.GetHeaders(name).Select(h => h.Value).ToArray();
}