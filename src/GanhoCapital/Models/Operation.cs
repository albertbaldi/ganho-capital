using System.Text.Json.Serialization;

namespace GanhoCapital.Models;

public class Operation
{
    [JsonPropertyName("operation")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("unit-cost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
