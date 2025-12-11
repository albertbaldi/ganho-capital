using System.Text.Json;
using GanhoCapital.Models;
using GanhoCapital.Services;

var calculator = new TaxCalculator();
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

string? line;
while ((line = Console.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line))
        break;

    try
    {
        var operations = JsonSerializer.Deserialize<List<Operation>>(line, options);
        if (operations == null || operations.Count == 0)
            continue;

        var results = calculator.CalculateTaxes(operations);
        var output = JsonSerializer.Serialize(results, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });

        Console.WriteLine(output);
    }
    catch (JsonException)
    {
        // Invalid JSON, skip
    }
}
