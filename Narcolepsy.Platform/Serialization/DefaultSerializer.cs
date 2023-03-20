namespace Narcolepsy.Platform.Serialization; 
using System;
using System.Text.Json;
using System.Threading.Tasks;

internal class DefaultSerializer : ISerializer {
    public bool CanSerialize(Type type) => true;

    public Task<T> DeserializeAsync<T>(ReadOnlySpan<byte> serialized)
        => Task.FromResult(JsonSerializer.Deserialize<T>(serialized) ?? throw new NotImplementedException("todo"));

    public async Task<byte[]> SerializeAsync<T>(T obj) {
        using MemoryStream WriteStream = new();
        await JsonSerializer.SerializeAsync(WriteStream, obj);
        return WriteStream.ToArray();
    }
}
