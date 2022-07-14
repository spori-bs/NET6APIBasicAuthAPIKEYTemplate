using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net6APIBasicAuthApiKey.Helpers;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var data = reader.GetString();
        var dataArray = data?.Split('.');
        return dataArray != null && dataArray.Length == 3
            ? new DateOnly(short.Parse(dataArray[0]), short.Parse(dataArray[1]), short.Parse(dataArray[2]))
            : throw new FormatException($"The passed value ('{data}') is not in YYYY.MM.DD format");
    }
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy.MM.dd"));
    }
}