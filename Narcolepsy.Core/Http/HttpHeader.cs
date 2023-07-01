namespace Narcolepsy.Core.Http;

using System.Text.RegularExpressions;

public partial record HttpHeader(string Name, string Value, bool IsEnabled, bool IsUserModifiable, string? Note) {
    private readonly Regex ValidNameRegex = HttpHeader.GetValidTokenRegex();

    public bool IsNameValid => this.ValidNameRegex.IsMatch(this.Name);

    public bool IsValueValid => true; // todo

    [GeneratedRegex("^[!#$%&'*+\\-.0123456789A-Z^_`a-z|]*$")]
    private static partial Regex GetValidTokenRegex();
}