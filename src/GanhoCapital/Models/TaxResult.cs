using System.Text.Json.Serialization;

namespace GanhoCapital.Models;

public class TaxResult
{
    [JsonPropertyName("tax")]
    [JsonConverter(typeof(DecimalJsonConverter))]
    public decimal Tax { get; set; }
}
