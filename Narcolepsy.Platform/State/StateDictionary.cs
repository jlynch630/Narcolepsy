namespace Narcolepsy.Platform.State; 
using System.Collections.Generic;
using System.Text.Json.Serialization;

[JsonConverter(typeof(StateDictionaryJsonConverter))]
public class StateDictionary {
	internal Dictionary<string, SerializableState> StateStore;

	public StateDictionary() => this.StateStore = new Dictionary<string, SerializableState>();

	internal StateDictionary(Dictionary<string, SerializableState> stateStore)
		=> this.StateStore = stateStore;

    public void Add<T>(string key, T value, bool persist = true) {
        this.StateStore[key] = new SerializableState(value, persist);
    }

    public T Get<T>(string key, T defaultValue)
		=> this.StateStore.TryGetValue(key, out SerializableState? Value) ? (T)Value.Value! : defaultValue;

	public void Remove(string key) => this.StateStore.Remove(key);
}
