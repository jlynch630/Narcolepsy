namespace Narcolepsy.Core.Http.Exceptions;

public class InvalidRequestException : Exception {
    public InvalidRequestException(InvalidRequestType type) : this(type, null) { }

    public InvalidRequestException(InvalidRequestType type, string? message) : this(type, message, null) { }

    public InvalidRequestException(InvalidRequestType type, string? message, Exception? innerException) : base(message,
        innerException) => this.Type = type;

    public InvalidRequestType Type { get; }
}

public enum InvalidRequestType {
    Headers,
    Other
}