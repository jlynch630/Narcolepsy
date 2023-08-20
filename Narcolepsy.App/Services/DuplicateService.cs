namespace Narcolepsy.App.Services {
    using Narcolepsy.App.Plugins.Requests;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Platform.Requests;
    using Platform.Serialization;
    using Narcolepsy.Platform.Logging;

    internal class DuplicateService {
        private readonly RequestManager RequestManager;
        private readonly SerializationManager SerializationManager;

        public DuplicateService(RequestManager requestManager, SerializationManager serializer) {
            this.RequestManager = requestManager;
            this.SerializationManager = serializer;
        }

        public async Task<Request> DuplicateAsync(string name, string type, IRequestContext source) {
            RequestSnapshot Snapshot = await this.SerializationManager.SerializeRequestAsync(type, source);
            Request Result = await this.RequestManager.CreateRequestAsync(Snapshot);
            Result.Context.Name.Value = name;
            return Result;
        }
    }
}
