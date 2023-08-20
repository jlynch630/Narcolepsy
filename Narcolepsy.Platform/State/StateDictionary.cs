namespace Narcolepsy.Platform.State;

using Narcolepsy.Platform.Logging;
using System.Text.Json.Serialization;

[JsonConverter(typeof(StateDictionaryJsonConverter))]
public class StateDictionary {
    internal Dictionary<string, SerializableState> StateStore;

    public StateDictionary() => this.StateStore = new Dictionary<string, SerializableState>();

    internal StateDictionary(Dictionary<string, SerializableState> stateStore)
        => this.StateStore = stateStore;

    public void Add<T>(string key, T value, bool persist = true)
        => this.StateStore[key] = new SerializableState(value, persist);

    public T Get<T>(string key, T defaultValue) {
        if (!this.StateStore.TryGetValue(key, out SerializableState? Value)) return defaultValue;
        if (Value.Value is null) return defaultValue;

        Type SavedType = Value.Value.GetType();
        Type TargetType = typeof(T);
        if (SavedType.IsAssignableTo(TargetType)) return (T)Value.Value;

        Logger.Debug("Failed to get {Key} from storage, could not assign type {Saved} to type {Target}", key, SavedType, TargetType);
        return defaultValue;
    }

    public void Remove(string key) => this.StateStore.Remove(key);
}