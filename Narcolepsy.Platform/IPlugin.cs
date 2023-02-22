namespace Narcolepsy.Platform;

using Microsoft.Extensions.DependencyInjection;

public interface IPlugin {
    public string FullName { get; }

    public string Description { get; }

    public PluginVersion Version { get; }

    public Task InitializeAsync(NarcolepsyContext context);
}