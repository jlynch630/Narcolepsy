namespace Narcolepsy.App.Plugins.Requests;
using System;

internal class RequestConfigurationException : Exception {
    public RequestConfigurationException(string message) : base(message) { }

    public RequestConfigurationException(string message, Exception inner) : base(message, inner) { }
}

