namespace Narcolepsy.Core.Http;

public record RequestExecutionError(string? Message, string? HelpText, Exception? Exception);