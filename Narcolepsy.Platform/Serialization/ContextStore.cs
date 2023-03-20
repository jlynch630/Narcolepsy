namespace Narcolepsy.Platform.Serialization {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ContextStore : IContextStore {
        private ISerializer Serializer;

        public ContextStore(ISerializer serializer) {
            this.Serializer = serializer;
        }

        public Task<byte[]>? SerializedTask { get; private set; } = null;

        public void Put<T>(T saveState) {
            // because we want to keep track of the type parameter, we must serialize here
            this.SerializedTask = this.Serializer.SerializeAsync(saveState);
        }
    }
}
