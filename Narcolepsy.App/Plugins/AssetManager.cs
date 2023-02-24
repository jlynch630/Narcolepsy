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
	public void InjectStylesheet(string path) => this.InjectStylesheet(path, Assembly.GetCallingAssembly().GetName().Name);
    public void InjectStylesheet(string path, string packageId) => this.ScriptsToLoad.Add($"_content/{packageId}/{path}");

    public async Task LoadAllScriptsAsync(IJSRuntime jsRuntime) {
		foreach (string Script in this.ScriptsToLoad)
			await jsRuntime.InvokeVoidAsync("injectScript", Script);
	}

	public async Task LoadAllStylesAsync(IJSRuntime jsRuntime) {
		foreach (string Stylesheet in this.StylesToLoad)
			await jsRuntime.InvokeVoidAsync("injectStyle", Stylesheet);
	}
}