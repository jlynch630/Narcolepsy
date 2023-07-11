namespace Narcolepsy.App.Plugins; 

internal class PluginLoadException : Exception {
    public PluginLoadException(string message) : base(message) { }

    public PluginLoadException(string message, Exception innerException) : base(message, innerException) { }
}