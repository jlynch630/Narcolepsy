namespace Narcolepsy.App.Plugins;

using System.Reflection;
using Microsoft.JSInterop;
using Platform;

internal class AssetManager : IAssetManager {
    private readonly List<string> ScriptsToLoad = new();
    private readonly List<string> StylesToLoad = new();

    public void InjectScript(string path, string packageId) {
        string QualifiedPath = $"_content/{packageId}/{path}";
        this.ScriptsToLoad.Add(QualifiedPath);
    }

    public void InjectScript(string path) => this.InjectScript(path, Assembly.GetCallingAssembly().GetName().Name);

    public void InjectStylesheet(string path) =>
        this.InjectStylesheet(path, Assembly.GetCallingAssembly().GetName().Name);

    public void InjectStylesheet(string path, string packageId) =>
        this.StylesToLoad.Add($"_content/{packageId}/{path}");

    public async Task LoadAllScriptsAsync(IJSRuntime jsRuntime) {
        if (!this.ScriptsToLoad.Any()) return;
        await jsRuntime.InvokeVoidAsync("injectScripts", String.Join("\0", this.ScriptsToLoad));
    }

    public async Task LoadAllStylesAsync(IJSRuntime jsRuntime) {
        foreach (string Stylesheet in this.StylesToLoad)
            await jsRuntime.InvokeVoidAsync("injectStyle", Stylesheet);
    }
}