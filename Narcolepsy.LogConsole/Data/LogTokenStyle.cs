namespace Narcolepsy.LogConsole.Data;

using System.Diagnostics.CodeAnalysis;

public record LogTokenStyle([AllowNull] string Foreground, [AllowNull] string Background, bool Italic, bool Underline, bool Bold) {
    public static readonly LogTokenStyle None = new(null, null, false, false, false);

    public string GetCssStyle() =>
        $"color: {this.Foreground ?? "unset"}; background-color: {this.Background ?? "unset"}; text-decoration: {(this.Underline ? "underline" : "none")}; font-weight: {(this.Bold ? "bold" : "normal")}; font-style: {(this.Italic ? "italic" : "normal")};";
}

