namespace Narcolepsy.Core.Http;

using System.Text.RegularExpressions;

public partial record HttpHeader(string Name, string Value, bool IsEnabled, bool IsUserModifiable, string? Note) {
    private Regex ValidNameRegex = GetValidTokenRegex();

    public bool IsNameValid => ValidNameRegex.IsMatch(Name);

    public bool IsValueValid => true;

    [GeneratedRegex("^[!#$%&'*+\\-.0123456789A-Z^_`a-z|]*$")]
    private static partial Regex GetValidTokenRegex();
}