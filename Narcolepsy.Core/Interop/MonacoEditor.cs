namespace Narcolepsy.Core.Interop {
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public class MonacoEditor {
        private readonly IJSObjectReference JsReference;
        private readonly IJSRuntime JsRuntime;

        internal MonacoEditor(IJSObjectReference reference, IJSRuntime jSRuntime) {
            this.JsReference = reference;
            this.JsRuntime = jSRuntime;
        }

        public ValueTask SetModel(MonacoTextModel model) => this.JsReference.InvokeVoidAsync("setModel", model.JsReference);

        public ValueTask Trigger(string source, string action)
             => this.JsReference.InvokeVoidAsync("trigger", source, action);
    }
}
