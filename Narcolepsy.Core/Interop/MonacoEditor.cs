namespace Narcolepsy.Core.Interop {
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public class MonacoEditor {
        private readonly IJSObjectReference JsReference;
        private readonly IJSRuntime JsRuntime;
        private readonly DotNetObjectReference<MonacoEditor> ThisReference;

        private event EventHandler<string>? ModelContentChangedEvent;

        public event EventHandler<string>? ModelContentChanged {
            add {
                this.ListenForModelChanges();
                this.ModelContentChangedEvent += value;
            }
            remove {
                this.ModelContentChangedEvent -= value;
            }
        }

        internal MonacoEditor(IJSObjectReference reference, IJSRuntime jSRuntime) {
            this.JsReference = reference;
            this.JsRuntime = jSRuntime;
            this.ThisReference = DotNetObjectReference.Create(this);
        }

        public ValueTask SetModel(MonacoTextModel model) => this.JsReference.InvokeVoidAsync("setModel", model.JsReference);

        public ValueTask Trigger(string source, string action)
             => this.JsReference.InvokeVoidAsync("trigger", source, action);

        public ValueTask<string> GetValue() => this.JsReference.InvokeAsync<string>("getValue", new { lineEnding = "\r\n", preserveBOM = false });

        private ValueTask ListenForModelChanges() => this.JsRuntime.InvokeVoidAsync("narcolepsy.listenToEditorChanges", this.JsReference, this.ThisReference);

        [JSInvokable]
        public void InvokeModelContentChanged(string value) {
            this.ModelContentChangedEvent?.Invoke(this, value);
        }
    }
}
