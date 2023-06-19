using Otel.Demo.AssetApi.Models;
using Otel.Demo.AssetApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;

namespace Otel.Demo.AssetApi.Controllers
{
    [Route("asset")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private ILogger _logger;
        private readonly ITelemetryService _telemetryService;
        private readonly IAssetService _assetService;
        private readonly IUserService _userService;

        public AssetController(ILogger<AssetController> logger, ITelemetryService telemetryService,
            IAssetService assetService, IUserService userService)
        {
            _logger = logger;
            _telemetryService = telemetryService;
            _assetService = assetService;
            _userService = userService;
        }

        [HttpGet("GetSampleValue")]
        public IActionResult GetSampleValue()
        {
            return Ok(2);
        }

        [HttpGet("GetAssetDataSeq/{assetId}")]
        public async Task<IActionResult> GetAssetDataSeq(string assetId = "4de1208e-d1b7-46a1-9743-8f2b39c3ad39")
        {
            _logger.LogInformation("Entering GetAssetDataSeq");

            _telemetryService.GetAssetDataSeqReqCounter().Add(1,
                new("Action", nameof(GetAssetDataSeq)),
                new("Controller", nameof(AssetController)));

            string? username = await _userService.GetUserName();

            var contextId = Baggage.GetBaggage("ContextId");
            if (string.IsNullOrEmpty(contextId))
            {
                contextId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation($"GetAssetDataSeq : {username} : {assetId}");

            using var activity_GetAssetDataSeq = _telemetryService.GetActivitySource().StartActivity("GetAssetDataSeq");
            activity_GetAssetDataSeq?.SetTag("AssetId", assetId);
            activity_GetAssetDataSeq?.SetTag("ContextId", contextId);
            activity_GetAssetDataSeq?.AddEvent(new("GetAssetData"));
            Baggage.SetBaggage("ContextId", contextId.ToString());

            var assetDetails = await _assetService.GetAssetDetails(assetId);
            var variableData = await _assetService.GetVariableDataSeq(assetDetails);
            var eventData = await _assetService.GetEventData(assetId);

            AssetData assetData = new AssetData();
            assetData.AssetId = assetId.ToString();
            assetData.AssetName= assetDetails?["name"]?.ToString();
            assetData.Username = username;
            assetData.VariableData = variableData;
            assetData.EventData = eventData;
            _logger.LogInformation("Exiting GetAssetDataSeq");
            return Ok(assetData);
        }

        [HttpGet("GetAssetData/{assetId}")]
        public async Task<IActionResult> GetAssetData(string assetId = "4de1208e-d1b7-46a1-9743-8f2b39c3ad39")
        {
            _logger.LogInformation("Entering GetAssetData");

            _telemetryService.GetAssetDataReqCounter().Add(1,
                new("Action", nameof(GetAssetData)),
                new("Controller", nameof(AssetController)));

            string? username = await _userService.GetUserName();

            var contextId = Baggage.GetBaggage("ContextId");
            if (string.IsNullOrEmpty(contextId))
            {
                contextId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation($"GetAssetDataSeq : {username} : {assetId}");

            using var activity_GetAssetData = _telemetryService.GetActivitySource().StartActivity("GetAssetData");
            activity_GetAssetData?.SetTag("AssetId", assetId);
            activity_GetAssetData?.SetTag("ContextId", contextId);
            activity_GetAssetData?.AddEvent(new("GetAssetData"));
            Baggage.SetBaggage("ContextId", contextId.ToString());

            var assetDetails = await _assetService.GetAssetDetails(assetId);
            var variableDataTask = _assetService.GetVariableData(assetDetails);
            var eventDataTask = _assetService.GetEventData(assetId);
            await Task.WhenAll(variableDataTask, eventDataTask);

            AssetData assetData = new AssetData();
            assetData.AssetId = assetId.ToString();
            assetData.AssetName= assetDetails?["assetName"]?.ToString();
            assetData.Username = username;
            assetData.VariableData = await variableDataTask;
            assetData.EventData = await eventDataTask;
            _logger.LogInformation("Exiting GetAssetData");
            return Ok(assetData);
        }
    }
}
