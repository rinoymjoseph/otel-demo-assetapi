namespace Otel.Demo.AssetApi
{
    public class AppConstants
    {
        public const string OTEL_SERVCICE_NAME = "AssetApi";

        public const string OTEL_EXPORTER_URL = "Otel:ExporterUrl";
        public const string OTEL_ENABLE_LOGGING = "Otel:EnableLogging";
        public const string OTEL_ENABLE_TRACING = "Otel:EnableTracing";
        public const string OTEL_ENABLE_METRICS = "Otel:EnableMetrics";

        public const string DATA_API_URL = "DataApiUrl";
        public const string VARIABLE_API_URL = "VariableApiUrl";
        public const string EVENT_API_URL = "EventApiUrl";
        public const string ALARM_API_URL = "AlarmApiurl";

        public const string REQUEST_GET_ASSET_DETAILS = "/assetdata/GetAssetDetails";
        public const string REQUEST_GET_VARIABLE_DATA = "/variable/GetVariableData";
        public const string REQUEST_GET_EVENT_DATA = "/event/GetEventsOfAsset";
        public const string REQUEST_GET_USERNAME = "/userdata/GetUserName";

        public const string COUNTER_ASSET_GET_ASSET_DATA = "asset_api_get_asset_data_requests";
        public const string COUNTER_ASSET_GET_ASSET_DATA_REQUESTS_FAILURE = "asset_api_get_asset_data_requests_failure";
        public const string COUNTER_ASSET_GET_ASSET_DATA_REQUESTS_SUCCESS= "asset_api_get_asset_data_requests_success";
        public const string COUNTER_ASSET_GET_ASSET_DATA_SEQ = "asset_api_get_asset_data_seq_requests";
    }
}
