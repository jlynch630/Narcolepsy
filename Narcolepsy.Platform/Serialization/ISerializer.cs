namespace Narcolepsy.Platform.Serialization; 
using System;
using System.Threading.Tasks;

public interface ISerializer {
    public bool CanSerialize(Type type);

    public Task<byte[]> SerializeAsync<T>(T obj);

    public Task<T> DeserializeAsync<T>(ReadOnlySpan<byte> serialized);
}
