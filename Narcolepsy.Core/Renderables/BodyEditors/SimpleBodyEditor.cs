namespace Narcolepsy.Core.Renderables.BodyEditors {
    using Microsoft.AspNetCore.Components;
    using Narcolepsy.Core.Http;
    using Narcolepsy.Platform.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SimpleBodyEditor<TComponent> : Renderable<TComponent, IHttpRequestContext>, IBodyEditor where TComponent : IComponent {
        public string Name { get; }

        public SimpleBodyEditor(string name) {
            this.Name = name;
        }
    }
}
