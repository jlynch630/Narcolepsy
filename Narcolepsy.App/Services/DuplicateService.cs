namespace Narcolepsy.App.Services {
    using Narcolepsy.App.Plugins.Requests;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Platform.Requests;
    using Platform.Serialization;

    internal class DuplicateService {
        private readonly RequestManager RequestManager;

        public DuplicateService(RequestManager requestManager) {
            this.RequestManager = requestManager;
        }

        public Task<Request> DuplicateAsync(string name, string type, IRequestContext source) {
            DuplicateContextStore ContextStore = new();
            source.Save(ContextStore);
            return this.RequestManager.CreateRequestAsync(name, type, ContextStore.SaveState);
        }
    }
}
