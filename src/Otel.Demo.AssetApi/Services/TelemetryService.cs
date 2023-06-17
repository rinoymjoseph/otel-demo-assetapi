using Otel.Demo.AssetApi.Services.Interfaces;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Otel.Demo.AssetApi.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ActivitySource _activitySource;
        public static Meter _meter = new(AppConstants.OTEL_SERVCICE_NAME);
        private readonly Counter<long> _assetDataReqCounter;
        private readonly Counter<long> _assetDataReqSeqCounter;

        public TelemetryService()
        {
            _activitySource = new ActivitySource(AppConstants.OTEL_SERVCICE_NAME);
            _assetDataReqCounter = _meter.CreateCounter<long>(AppConstants.COUNTER_ASSET_GET_ASSET_DATA);
            _assetDataReqSeqCounter = _meter.CreateCounter<long>(AppConstants.COUNTER_ASSET_GET_ASSET_DATA_SEQ);
        }

        public ActivitySource GetActivitySource()
        {
            return _activitySource;
        }

        public Counter<long> GetAssetDataReqCounter()
        {
            return _assetDataReqCounter;
        }

        public Counter<long> GetAssetDataSeqReqCounter()
        {
            return _assetDataReqSeqCounter;
        }
    }
}
