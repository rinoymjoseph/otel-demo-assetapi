using System.Text.Json.Nodes;

namespace Otel.Demo.AssetApi.Models
{
    public class AssetData
    {
        public string? AssetId { get; set; }
        public string? AssetName { get; set; }
        public string? Username { get; set; }
        public JsonArray? VariableData { get; set; }
        public JsonArray? AlarmData { get; set; }
        public JsonArray? EventData { get; set; }
    }
}
