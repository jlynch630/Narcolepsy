namespace Narcolepsy.Core.Http;

public static class HttpHeaderListExtensions {
    public static void AddHeader(this IList<HttpHeader> list, string name, string value, bool enabled = true,
                                 bool userModifiable = true, string? note = null) {
        list.Add(new HttpHeader(name, value, enabled, userModifiable, note));
    }

    public static HttpHeader? GetHeader(this IList<HttpHeader> list, string name)
        => list.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public static HttpHeader[] GetHeaders(this IList<HttpHeader> list, string name)
        => list.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();

    public static void RemoveAutoHeader(this IList<HttpHeader> list, string name) {
        // only remove if we set it
        IEnumerable<HttpHeader> Existing = list
                                           .Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                                       c.Note is not null).ToArray();

        foreach (HttpHeader Header in Existing)
            list.Remove(Header);
    }

    public static void RemoveHeader(this ICollection<HttpHeader> list, string name) {
        foreach (HttpHeader Existing in list.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                            .ToArray())
            list.Remove(Existing);
    }

    public static void SetAutoHeader(this IList<HttpHeader> list, string name, string value, string note,
                                     bool userModifiable = true) {
        // only replace if the existing one is also auto-set
        HttpHeader? Existing = list.GetHeader(name);
        if (Existing is not null && Existing.Note is null) return;

        // otherwise, we are free to replace!
        list.SetHeader(name, value, true, userModifiable, note);
    }

    public static void SetHeader(this IList<HttpHeader> list, string name, string value, bool enabled = true,
                                 bool userModifiable = true, string? note = null) {
        HttpHeader[] Existing = list.GetHeaders(name);

        int InsertIndex = list.Count;
        if (Existing.Length > 0) {
            InsertIndex = list.IndexOf(Existing[0]);

            // first remove the existing header
            foreach (HttpHeader ExistingHeader in Existing)
                list.Remove(ExistingHeader);
        }

        // then add our new one
        list.Insert(InsertIndex, new HttpHeader(name, value, enabled, userModifiable, note));
    }
}