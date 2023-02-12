namespace Narcolepsy.Core.Http; 

public record HttpHeader(string Name, string Value, bool IsEnabled, bool IsUserModifiable, string? Note);