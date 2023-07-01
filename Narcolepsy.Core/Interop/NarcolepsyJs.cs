namespace Narcolepsy.Core.Interop;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public class NarcolepsyJs {
    private readonly IJSRuntime JsRuntime;
    private readonly Lazy<Task<IJSObjectReference>> NarcolespyModule;

    public NarcolepsyJs(IJSRuntime jsRuntime) {
        this.NarcolespyModule = new Lazy<Task<IJSObjectReference>>(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Narcolepsy.Core/script/index.js").AsTask());
        this.JsRuntime = jsRuntime;
    }

    public async Task<MarkupString> Colorize(string value, string language) {
        IJSObjectReference Module = await this.NarcolespyModule.Value;
        return (MarkupString)await Module.InvokeAsync<string>("colorize", value, language);
    }

    public async Task<MonacoEditor> CreateEditor(ElementReference element, string language, bool isReadOnly,
                                                 string? initialValue = null) {
        IJSObjectReference Module = await this.NarcolespyModule.Value;
        IJSObjectReference Ref = await Module.InvokeAsync<IJSObjectReference>("createEditor",
            element, language, isReadOnly, initialValue);
        return new MonacoEditor(Ref, Module);
    }

    public async Task<MonacoTextModel> CreateTextModel(string value, string language) {
        IJSObjectReference Module = await this.NarcolespyModule.Value;
        IJSObjectReference Ref =
            await Module.InvokeAsync<IJSObjectReference>("createModel", value, language);
        return new MonacoTextModel(Ref);
    }

    public async Task<string> FormatCode(string value, string extension) {
        IJSObjectReference Module = await this.NarcolespyModule.Value;
        return await Module.InvokeAsync<string>("format", value, extension);
    }
}