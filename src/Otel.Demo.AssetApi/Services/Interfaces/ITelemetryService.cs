using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Otel.Demo.AssetApi.Services.Interfaces
{
    public interface ITelemetryService
    {
        ActivitySource GetActivitySource();

        Counter<long> GetAssetDataReqCounter();

        Counter<long> GetAssetDataSeqReqCounter();

        Counter<long> GetAssetDataReqCounterSuccess();

        Counter<long> GetAssetDataReqCounterFailure();
    }
}
