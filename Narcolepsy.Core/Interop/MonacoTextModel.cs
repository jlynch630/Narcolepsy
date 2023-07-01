namespace Narcolepsy.Core.Interop;

using Microsoft.JSInterop;

public class MonacoTextModel {
    internal MonacoTextModel(IJSObjectReference reference) => this.JsReference = reference;

    internal IJSObjectReference JsReference { get; }
}