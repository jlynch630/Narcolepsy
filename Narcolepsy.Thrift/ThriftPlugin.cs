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
    using Platform;
    using Platform.Extensions;

    public class ThriftPlugin : IPlugin {
        public string FullName => "Thrift";

        public string Description => "";

        public PluginVersion Version => new(0, 0, 1);

        public async Task InitializeAsync(NarcolepsyContext context) {
            context.Requests.ConfigureHttp(config => {
                config
                    .AddRequestBodyEditor<ThriftBodyEditorView>("Thrift");
            });
        }
    }
}
