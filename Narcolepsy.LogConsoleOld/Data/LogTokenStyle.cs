namespace Narcolepsy.LogConsole.Data;

using System.Diagnostics.CodeAnalysis;

internal record LogTokenStyle([AllowNull] string Foreground, [AllowNull] string Background, bool Italic, bool Underline, bool Bold) {
    public static readonly LogTokenStyle None = new(null, null, false, false, false);
}

