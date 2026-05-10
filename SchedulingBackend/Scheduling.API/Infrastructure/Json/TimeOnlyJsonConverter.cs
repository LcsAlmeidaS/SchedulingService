using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduling.API.Infrastructure.Json;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm:ss";

    private static readonly string[] AcceptedFormats = ["HH:mm:ss", "HH:mm:ss.fff", "HH:mm"];

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!.TrimEnd('Z');
        return TimeOnly.ParseExact(value, AcceptedFormats, null);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}
