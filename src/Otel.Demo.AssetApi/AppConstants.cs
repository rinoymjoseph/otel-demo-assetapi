namespace Otel.Demo.AssetApi
{
    public class AppConstants
    {
        public const string OTEL_SERVCICE_NAME = "AssetApi";

        public const string URL_OTEL_EXPORTER = "otel_exporter_url";
        public const string URL_DATA_API = "data_api_url";
        public const string URL_VARIABLE_API = "varialbe_api_url";
        public const string URL_EVENT_API = "event_api_url";
        public const string URL_ALARM_API = "alarm_api_url";

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
