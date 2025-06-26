namespace Narcolepsy.CoinbaseCDP;

using Platform;
using Platform.Extensions;

public class CdpPlugin : IPlugin {
    public string Description => "Authenticates requests to the Coinbase CDP API";

    public string FullName => "Coinbase CDP";

    public PluginVersion Version { get; } = new(0, 0, 1);

    public Task InitializeAsync(NarcolepsyContext context) {
        context.Requests.ConfigureHttp(v => { v.AddRequestTab<CoinbaseTab>("Coinbase"); });
        return Task.CompletedTask;
    }
}