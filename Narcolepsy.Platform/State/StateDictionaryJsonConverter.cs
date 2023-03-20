namespace Narcolepsy.Platform.State {
	using System;
	using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
	using System.Text;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using System.Threading.Tasks;

	internal class StateDictionaryJsonConverter : JsonConverter<StateDictionary> {
		public override StateDictionary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			// okay what we have to do is read 
            Dictionary<string, SerializableState>? StateStore = this.GetDictionaryConverter(options).Read(ref reader, typeof(Dictionary<string, SerializableState>), options);
            return StateStore is null ? new StateDictionary() : new StateDictionary(new Dictionary<string, SerializableState>(StateStore));
		}

		public override void Write(Utf8JsonWriter writer, StateDictionary value, JsonSerializerOptions options)
			=> this.GetIDictionaryConverter(options).Write(writer, value.StateStore.Where(v => v.Value.Persist).ToImmutableDictionary(), options);

        private JsonConverter<Dictionary<string, SerializableState>> GetDictionaryConverter(JsonSerializerOptions options)
            => (JsonConverter<Dictionary<string, SerializableState>>)options.GetConverter(typeof(Dictionary<string, SerializableState>));

        private JsonConverter<IDictionary<string, SerializableState>> GetIDictionaryConverter(JsonSerializerOptions options)
			=> (JsonConverter<IDictionary<string, SerializableState>>)options.GetConverter(typeof(IDictionary<string, SerializableState>));
    }
}
