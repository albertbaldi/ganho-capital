using System.Text.Json;
using System.Text.Json.Serialization;

namespace GanhoCapital.Models;

public class DecimalJsonConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDecimal();
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
    }
}
