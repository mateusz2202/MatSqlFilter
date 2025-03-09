using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatSqlFilter;

internal class FilterComponentConverter : JsonConverter<IFilterComponent>
{
    public override IFilterComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement root = JsonElement.ParseValue(ref reader);

        if (root.TryGetProperty("Column", out _))
            return JsonSerializer.Deserialize<Condition>(root.GetRawText(), options);


        if (root.TryGetProperty("Filters", out _))
            return JsonSerializer.Deserialize<Filter>(root.GetRawText(), options);

        throw new JsonException("Unknown type of IFilterComponent");
    }

    public override void Write(Utf8JsonWriter writer, IFilterComponent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
