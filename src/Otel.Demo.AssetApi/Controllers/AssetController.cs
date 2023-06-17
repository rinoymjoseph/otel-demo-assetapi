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
        private readonly ITelemetryService _telemetryService;
        private readonly IAssetService _assetService;

        public AssetController(IConfiguration configuration, IHttpClientFactory httpClientFactory,
           ITelemetryService telemetryService, IAssetService assetService)
        {
            _telemetryService = telemetryService;
            _assetService = assetService;
        }

        [HttpGet("GetSampleValue")]
        public IActionResult GetSampleValue()
        {
            return Ok(2);
        }

        [HttpGet("GetAssetDataSeq/{assetId}")]
        public async Task<IActionResult> GetAssetDataSeq(string assetId = "4de1208e-d1b7-46a1-9743-8f2b39c3ad39")
        {
            _telemetryService.GetAssetDataSeqReqCounter().Add(1,
                new("Action", nameof(GetAssetDataSeq)),
                new("Controller", nameof(AssetController)));

            var contextId = Baggage.GetBaggage("ContextId");
            if (string.IsNullOrEmpty(contextId))
            {
                contextId = Guid.NewGuid().ToString();
            }
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
            assetData.VariableData = variableData;
            assetData.EventData = eventData;
            return Ok(assetData);
        }

        [HttpGet("GetAssetData/{assetId}")]
        public async Task<IActionResult> GetAssetData(string assetId = "4de1208e-d1b7-46a1-9743-8f2b39c3ad39")
        {
            _telemetryService.GetAssetDataReqCounter().Add(1,
                new("Action", nameof(GetAssetData)),
                new("Controller", nameof(AssetController)));
            var contextId = Baggage.GetBaggage("ContextId");
            if (string.IsNullOrEmpty(contextId))
            {
                contextId = Guid.NewGuid().ToString();
            }
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
            assetData.AssetName= assetDetails?["name"]?.ToString();
            assetData.VariableData = await variableDataTask;
            assetData.EventData = await eventDataTask;
            return Ok(assetData);
        }
    }
}
