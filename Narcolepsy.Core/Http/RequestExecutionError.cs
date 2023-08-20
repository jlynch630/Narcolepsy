namespace Narcolepsy.Core.Http;

using System.Text.Json.Serialization;

public record RequestExecutionError(string? Message, string? HelpText, [property:JsonIgnore] Exception? Exception);