namespace Narcolepsy.Platform.State;

using System.Text.Json.Serialization;

[JsonConverter(typeof(SerializableStateJsonConverter))]
internal record SerializableState(object? Value, bool Persist);