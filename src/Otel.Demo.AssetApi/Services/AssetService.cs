using Otel.Demo.AssetApi.Services.Interfaces;
using System.Text.Json.Nodes;

namespace Otel.Demo.AssetApi.Services
{
    public class AssetService : IAssetService
    {
        private ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ITelemetryService _telemetryService;

        public AssetService(ILogger<AssetService> logger,
            IConfiguration configuration, IHttpClientFactory httpClientFactory,
           ITelemetryService telemetryService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _telemetryService = telemetryService;
        }

        public async Task<JsonObject?> GetAssetDetails(string assetId)
        {
            _logger.LogInformation($"Entering GetAssetDetails : assetId -> {assetId}");
            using var activity_getAssetDetails = _telemetryService.GetActivitySource().StartActivity("GetAssetDetails");
            var dataApiUrl = _configuration.GetValue<string>(AppConstants.URL_DATA_API);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{dataApiUrl}{AppConstants.REQUEST_GET_ASSET_DETAILS}/{assetId}");
            var httpClient = _httpClientFactory.CreateClient();
            var httpResult = await httpClient.SendAsync(request);
            var response = await httpResult.Content.ReadAsStringAsync();
            httpResult.EnsureSuccessStatusCode();
            var assetDetails = JsonNode.Parse(response)?.AsObject();
            _logger.LogInformation($"Exiting GetAssetDetails : assetId -> {assetId}");
            return assetDetails;
        }

        public async Task<JsonArray?> GetVariableDataSeq(JsonObject? assetDetails)
        {
            _logger.LogInformation("Entering GetVariableDataSeq");
            var variables = assetDetails?["variables"]?.AsArray();
            JsonArray? variableDataArray = new JsonArray();

            for (int i = 0; i < variables?.Count; i++)
            {
                var variable = variables[i]?.ToString();
                var variableDataObject = await GetVariableValue(variable);
                variableDataArray.Add(variableDataObject);
            }
            _logger.LogInformation("Exiting GetVariableDataSeq");
            return variableDataArray;
        }

        public async Task<JsonArray?> GetVariableData(JsonObject? assetDetails)
        {
            _logger.LogInformation("Entering GetVariableData");
            var variables = assetDetails?["variables"]?.AsArray();
            JsonArray? variableDataArray = new JsonArray();

            IList<Task<JsonObject?>> variableTasks = new List<Task<JsonObject?>>();

            for (int i = 0; i < variables?.Count; i++)
            {
                var variable = variables[i]?.ToString();
                var variableDataObjectTask = GetVariableValue(variable);
                variableTasks.Add(variableDataObjectTask);
            }
            await Task.WhenAll(variableTasks);

            foreach (var task in variableTasks)
            {
                variableDataArray.Add(await task);
            }
            _logger.LogInformation("Exiting GetVariableData");
            return variableDataArray;
        }

        private async Task<JsonObject?> GetVariableValue(string? variableName)
        {
            _logger.LogInformation($"Entering GetVariableValue : variableName -> {variableName}");
            var variableApiUrl = _configuration.GetValue<string>(AppConstants.URL_VARIABLE_API);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{variableApiUrl}{AppConstants.REQUEST_GET_VARIABLE_DATA}/{variableName}");
            var httpClient = _httpClientFactory.CreateClient();
            var httpResult = await httpClient.SendAsync(request);
            var response = await httpResult.Content.ReadAsStringAsync();
            httpResult.EnsureSuccessStatusCode();
            var variableData = JsonNode.Parse(response)?.AsObject();
            _logger.LogInformation($"Exiting GetVariableValue : variableName -> {variableName}");
            return variableData;
        }

        public async Task<JsonArray?> GetEventData(string asssetId)
        {
            _logger.LogInformation($"Entering GetEventData: asssetId -> {asssetId}");
            var eventApiUrl = _configuration.GetValue<string>(AppConstants.URL_EVENT_API);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{eventApiUrl}{AppConstants.REQUEST_GET_EVENT_DATA}/{asssetId}");
            var httpClient = _httpClientFactory.CreateClient();
            var httpResult = await httpClient.SendAsync(request);
            var response = await httpResult.Content.ReadAsStringAsync();
            httpResult.EnsureSuccessStatusCode();
            var eventData = JsonNode.Parse(response)?.AsArray();
            _logger.LogInformation($"Exiting GetEventData: asssetId -> {asssetId}");
            return eventData;
        }
    }
}
