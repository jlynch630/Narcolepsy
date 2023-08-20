namespace Narcolepsy.Thrift {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Components.Http.InfoBoxes;
    using Core.Components.Http.RequestTabs;
    using Core.Components.Http.ResponseTabs;
    using Narcolepsy.Core.Components.Http;
    using Narcolepsy.Core.Http;
    using Narcolepsy.Core.ViewConfig;
    using Narcolepsy.Platform.Requests;
    using Narcolepsy.UiKit.Form;
    using Narcolepsy.UiKit.Layout;
    using Platform;
    using Platform.Extensions;
    using Platform.Serialization;
    using Platform.State;

    public class ThriftPlugin : IPlugin {
        public string FullName => "Thrift";

        public string Description => "";

        public PluginVersion Version => new(0, 0, 1);

        public async Task InitializeAsync(NarcolepsyContext context) {
            context.Requests
                   .RegisterType<MyRequestContext, string, Button, ViewConf>("Thrift", save => new MyRequestContext(save ?? ""), new ViewConf())
                   .ConfigureIcon("play_arrow");
            context.Requests.ConfigureHttp(config => {
                config
                    .AddRequestBodyEditor<ThriftBodyEditorView>("Thrift", "Narcolepsy.Thrift-thrift");
            });
        }

        private record ViewConf() {}

        private class MyRequestContext : IRequestContext {
            public MyRequestContext(string name) {
                this.Name = new(name);
            }

            public MutableState<string> Name { get; }

            public void Save(IContextStore store) {
                store.Put(this.Name.Value);
            }
        }
    }
}
