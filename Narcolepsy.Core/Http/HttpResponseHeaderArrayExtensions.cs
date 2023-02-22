namespace Narcolepsy.Core.Http {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpResponseHeaderArrayExtensions {

        // todo: O(n)ness vs O(1) ness. who's to say, but will keep in mind.

        public static HttpResponseHeader[] GetHeaders(this HttpResponseHeader[] headers, string name) {
            return headers.Where(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public static string[] GetHeaderValues(this HttpResponseHeader[] headers, string name) => headers.GetHeaders(name).Select(h => h.Value).ToArray();

        public static string? GetHeaderValue(this HttpResponseHeader[] headers, string name) => headers.FirstOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
