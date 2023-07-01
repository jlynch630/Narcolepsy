namespace Narcolepsy.Core.Http.Body;

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

internal class HttpBodyJsonConverter : JsonConverter<IHttpBody> {
    public override IHttpBody? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType is JsonTokenType.Null) return null;
        if (reader.TokenType is not JsonTokenType.StartObject) throw new JsonException();

        JsonObject? Deserialized = JsonSerializer.Deserialize<JsonObject>(ref reader, options);
        if (Deserialized is null) return null;

        Type? Type = Type.GetType(Deserialized["$type"]?.GetValue<string>() ?? throw new JsonException());
        if (Type is null) throw new JsonException();

        return (IHttpBody?)Deserialized["value"]?.Deserialize(Type, options);
    }

    public override void Write(Utf8JsonWriter writer, IHttpBody value, JsonSerializerOptions options) {
        if (value is null) {
            writer.WriteNullValue();
            return;
        }

        Type Type = value.GetType();
        writer.WriteStartObject();
        writer.WriteString("$type", Type.AssemblyQualifiedName);
        writer.WritePropertyName("value");
        JsonSerializer.Serialize(writer, value, Type, options);
        writer.WriteEndObject();
    }
}