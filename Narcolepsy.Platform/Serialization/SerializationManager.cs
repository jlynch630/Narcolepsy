namespace Narcolepsy.Platform.Serialization;

using Narcolepsy.Platform.Logging;
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

    public Task<RequestSnapshot> SerializeRequestAsync(Request request) => this.SerializeRequestAsync(request.Type, request.Context);

    public async Task<RequestSnapshot> SerializeRequestAsync(string requestType, IRequestContext context) {
        ContextStore Store = new(this);
        context.Save(Store);
        if (Store.SerializedTask is null) {
#if DEBUG
            throw new RequestConfigurationException($"Failed to serialize {requestType} request. {context.GetType().Name}.Save did not call IContextStore.Put to store save data.");
#else
            Logger.Error("Failed to serialize {Type} request. {ContextName}.Save did not call IContextStore.Put to store save data", requestType, context.GetType().Name);
            return new RequestSnapshot(requestType, null);
#endif
        }
        byte[] Data = await (Store.SerializedTask);
        return new RequestSnapshot(requestType, Data);
    }

    private ISerializer SelectSerializer<T>() {
        Type SerializationType = typeof(T);

        return this.CustomSerializerList.TryGetValue(SerializationType, out ISerializer? Custom) ? Custom : this.DefaultSerializer;
    }
}
