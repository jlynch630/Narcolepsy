namespace Narcolepsy.Platform.Requests;

using System;

public class RequestConfigurationException : Exception {
    public RequestConfigurationException(string message) : base(message) { }

    public RequestConfigurationException(string message, Exception inner) : base(message, inner) { }
}

