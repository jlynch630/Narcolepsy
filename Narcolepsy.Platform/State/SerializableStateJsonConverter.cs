namespace Narcolepsy.Platform.State {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.Json;
	using System.Text.Json.Nodes;
	using System.Text.Json.Serialization;
	using System.Text.Json.Serialization.Metadata;
	using System.Threading.Tasks;

	internal class SerializableStateJsonConverter : JsonConverter<SerializableState> {
		public override SerializableState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			if (reader.TokenType != JsonTokenType.StartObject)
				throw new JsonException();

			Type? StateType = null;
			JsonNode? Value = null;

			while (reader.Read()) {
				if (reader.TokenType is JsonTokenType.EndObject) {
					// finish!
					if (StateType is null) throw new JsonException("Invalid or no type specified");
					return new SerializableState(Value.Deserialize(StateType, options), true);
				}

				if (reader.TokenType is not JsonTokenType.PropertyName) continue;
				string? PropertyName = reader.GetString();
				if (PropertyName is "$type") {
					reader.Read();
					string TypeString = reader.GetString() ?? throw new JsonException("Type cannot be null");
					if (TypeString is "null") return new SerializableState(null, true);

					StateType = Type.GetType(TypeString);
				} else if (PropertyName is "value") {
					// read the object into a jsonnode
					Value = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
					if (Value is null) return new SerializableState(null, true);
				} else
					throw new JsonException($"Unrecognized property name {PropertyName}. Acceptable values are \"$type\" and \"value\"");
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, SerializableState value, JsonSerializerOptions options) {
			if (value == null) throw new ArgumentNullException(nameof(value));

			// if we shouldn't persist the value, don't.
			if (!value.Persist) {
				writer.WriteNullValue();
				return;
			}

			// if our object is null, just write that
			if (value.Value is null) {
				writer.WriteStartObject();
				writer.WriteNull("$type");
				writer.WriteNull("value");
				writer.WriteEndObject();
				return;
			}

			// since our value is an object, we have to store the type with it!
			Type ValueType = value.Value.GetType();
			writer.WriteStartObject();
			writer.WriteString("$type", ValueType.AssemblyQualifiedName);
			writer.WritePropertyName("value");
			JsonSerializer.Serialize(writer, value.Value, options);
			writer.WriteEndObject();
		}
	}
}
