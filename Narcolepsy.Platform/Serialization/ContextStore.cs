namespace Narcolepsy.Platform.Serialization;

internal class ContextStore : IContextStore {
    private readonly ISerializer Serializer;

    public ContextStore(ISerializer serializer) => this.Serializer = serializer;

    public Task<byte[]>? SerializedTask { get; private set; }

    public void Put<T>(T saveState) {
        // because we want to keep track of the type parameter, we must serialize here
        this.SerializedTask = this.Serializer.SerializeAsync(saveState);
    }
}