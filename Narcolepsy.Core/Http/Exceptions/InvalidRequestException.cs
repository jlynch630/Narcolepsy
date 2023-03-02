namespace Narcolepsy.Core.Http.Exceptions; 
using System;
using System.Runtime.Serialization;

public class InvalidRequestException : Exception {
    public InvalidRequestType Type { get; }

    public InvalidRequestException(InvalidRequestType type) : this(type, null) {
    }

    public InvalidRequestException(InvalidRequestType type, string? message) : this(type, message, null) {
    }

    public InvalidRequestException(InvalidRequestType type, string? message, Exception? innerException) : base(message, innerException) {
        this.Type = type;
    }
}

public enum InvalidRequestType {
    Headers,
    Other
}