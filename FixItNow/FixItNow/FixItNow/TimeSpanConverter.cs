using Newtonsoft.Json;
using System;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        // Outputs "14:30:00"
        writer.WriteValue(value.ToString(@"hh\:mm\:ss"));
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return TimeSpan.Parse(reader.Value?.ToString() ?? "00:00:00");
    }
}
