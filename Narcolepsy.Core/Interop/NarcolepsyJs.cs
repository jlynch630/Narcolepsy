namespace Narcolepsy.Core.Interop;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public class NarcolepsyJs {
    private IJSRuntime JsRuntime;

    public NarcolepsyJs(IJSRuntime jsRuntime) => this.JsRuntime = jsRuntime;

    public async Task<MonacoEditor> CreateEditor(ElementReference element, string language, bool isReadOnly,
        string? initialValue = null) {
        IJSObjectReference Ref = await this.JsRuntime.InvokeAsync<IJSObjectReference>("narcolepsy.createEditor", element, language, isReadOnly, initialValue);
        return new MonacoEditor(Ref, JsRuntime);
    }

    public async Task<MonacoTextModel> CreateTextModel(string value, string language) {
        IJSObjectReference Ref = await this.JsRuntime.InvokeAsync<IJSObjectReference>("narcolepsy.createModel", value, language);
        return new MonacoTextModel(Ref);
    }

    public async Task<string> FormatCode(string value, string extension) => await this.JsRuntime.InvokeAsync<string>("narcolepsy.format", value, extension);

    public async Task<MarkupString> Colorize(string value, string language) => (MarkupString)await this.JsRuntime.InvokeAsync<string>("narcolepsy.colorize", value, language);
}