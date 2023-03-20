namespace Narcolepsy.Platform.Serialization;

using Narcolepsy.Platform.Requests;
using System;
public class SerializationManager : ISerializationManager, ISerializer {
    private ISerializer DefaultSerializer = new DefaultSerializer();
    private Dictionary<Type, ISerializer> CustomSerializerList = new();

    public void AddSerializer<T>(ISerializer serializer) {
        Type SerializationType = typeof(T);
        if (!serializer.CanSerialize(SerializationType))
            throw new NotImplementedException("todo");
           
        this.CustomSerializerList.Add(SerializationType, serializer);
    }

    public bool CanSerialize(Type type) => true;

    public Task<T> DeserializeAsync<T>(ReadOnlySpan<byte> serialized) => this.SelectSerializer<T>().DeserializeAsync<T>(serialized);

    public Task<byte[]> SerializeAsync<T>(T obj) => this.SelectSerializer<T>().SerializeAsync(obj);

    public async Task<RequestSnapshot> SerializeRequestAsync(Request request) {
        ContextStore Store = new ContextStore(this);
        request.Context.Save(Store);
        byte[] Data = await (Store.SerializedTask ?? throw new NotImplementedException("todo"));
        return new RequestSnapshot(request.Type, Data);
    }

    private ISerializer SelectSerializer<T>() {
        Type SerializationType = typeof(T);

        return this.CustomSerializerList.TryGetValue(SerializationType, out ISerializer? Custom) ? Custom : this.DefaultSerializer;
    }
}
