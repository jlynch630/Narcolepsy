﻿namespace Narcolepsy.Core.Http.Body;

using Narcolepsy.Platform.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

internal class HttpBodyJsonConverter : JsonConverter<IHttpBody> {
    public override IHttpBody? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType is JsonTokenType.Null) return null;
        if (reader.TokenType is not JsonTokenType.StartObject) throw new JsonException();

        JsonObject? Deserialized = JsonSerializer.Deserialize<JsonObject>(ref reader, options);
        if (Deserialized is null) return null;

        string? TypeName = Deserialized["$type"]?.GetValue<string>();
        Type? Type = Type.GetType(TypeName ?? "");
        if (Type is null) {
            Logger.Warning("Failed to deserialize IHttpBody of request. The type of the body was null. Either something went wrong during serialization or the plugin supplying the body is no longer installed. Body type: {Type}", TypeName);
            return null;
        }

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