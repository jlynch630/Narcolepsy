namespace Narcolepsy.Core.Interop;

using Microsoft.JSInterop;

public class MonacoEditor {
    private readonly IJSObjectReference JsReference;
    private readonly IJSObjectReference JsRuntime;
    private readonly DotNetObjectReference<MonacoEditor> ThisReference;

    internal MonacoEditor(IJSObjectReference reference, IJSObjectReference jsRuntime) {
        this.JsReference = reference;
        this.JsRuntime = jsRuntime;
        this.ThisReference = DotNetObjectReference.Create(this);
    }

    public ValueTask<string> GetValue() =>
        this.JsReference.InvokeAsync<string>("getValue", new { lineEnding = "\r\n", preserveBOM = false });

    [JSInvokable]
    public void InvokeModelContentChanged(string value) {
        this.ModelContentChangedEvent?.Invoke(this, value);
    }

    public ValueTask SetModel(MonacoTextModel model) => this.JsReference.InvokeVoidAsync("setModel", model.JsReference);

    public ValueTask Trigger(string source, string action)
        => this.JsReference.InvokeVoidAsync("trigger", source, action);

    private async void ListenForModelChanges() 
        => await this.JsRuntime.InvokeVoidAsync("listenToEditorChanges", this.JsReference, this.ThisReference);

    public event EventHandler<string>? ModelContentChanged {
        add {
            this.ListenForModelChanges();
            this.ModelContentChangedEvent += value;
        }
        remove => this.ModelContentChangedEvent -= value;
    }

    private event EventHandler<string>? ModelContentChangedEvent;
}