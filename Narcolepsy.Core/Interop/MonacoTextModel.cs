namespace Narcolepsy.Core.Interop {
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MonacoTextModel {
        internal IJSObjectReference JsReference { get; }

        internal MonacoTextModel(IJSObjectReference reference) => this.JsReference = reference;

    }
}
