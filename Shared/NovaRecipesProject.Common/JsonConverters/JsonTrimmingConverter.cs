using Newtonsoft.Json;

namespace NovaRecipesProject.Common.JsonConverters;

/// <inheritdoc />
public class JsonTrimmingConverter : JsonConverter
{
    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) => objectType == typeof(string);

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return ((string)reader.Value!)?.Trim();
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}