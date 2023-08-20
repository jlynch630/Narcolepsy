namespace Narcolepsy.Platform.Serialization;

using Narcolepsy.Platform.Requests;
using System;
using System.Text.Json;
using System.Threading.Tasks;

internal class DefaultSerializer : ISerializer {
    public bool CanSerialize(Type type) => true;

    public Task<T> DeserializeAsync<T>(ReadOnlySpan<byte> serialized)
        => Task.FromResult(JsonSerializer.Deserialize<T>(serialized) ?? throw new RequestConfigurationException($"Failed to deserialize request as type {typeof(T).Name}. Either the request was saved with a different/incompatible type or the save state cannot be deserialized without a custom ISerializer"));

    public async Task<byte[]> SerializeAsync<T>(T obj) {
        using MemoryStream WriteStream = new();
        await JsonSerializer.SerializeAsync(WriteStream, obj);
        return WriteStream.ToArray();
    }
}
