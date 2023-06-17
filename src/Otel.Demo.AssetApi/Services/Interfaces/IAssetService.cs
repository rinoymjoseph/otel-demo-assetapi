using System.Text.Json.Nodes;

namespace Otel.Demo.AssetApi.Services.Interfaces
{
    public interface IAssetService
    {
        Task<JsonObject?> GetAssetDetails(string assetId);

        Task<JsonArray?> GetVariableDataSeq(JsonObject? assetDetails);

        Task<JsonArray?> GetVariableData(JsonObject? assetDetails);

        Task<JsonArray?> GetEventData(string assetId);
    }
}
