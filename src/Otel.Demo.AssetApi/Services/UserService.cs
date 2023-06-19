using Otel.Demo.AssetApi.Services.Interfaces;

namespace Otel.Demo.AssetApi.Services
{
    public class UserService : IUserService
    {
        private ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ITelemetryService _telemetryService;

        public UserService(ILogger<UserService> logger,
            IConfiguration configuration, IHttpClientFactory httpClientFactory,
           ITelemetryService telemetryService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _telemetryService = telemetryService;
        }

        public async Task<string?> GetUserName()
        {
            _logger.LogInformation("Entering GetUserName");
            using var activity_getAssetDetails = _telemetryService.GetActivitySource().StartActivity("GetUserName");
            var dataApiUrl = _configuration.GetValue<string>(AppConstants.URL_DATA_API);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{dataApiUrl}{AppConstants.REQUEST_GET_USERNAME}");
            var httpClient = _httpClientFactory.CreateClient();
            var httpResult = await httpClient.SendAsync(request);
            var response = await httpResult.Content.ReadAsStringAsync();
            httpResult.EnsureSuccessStatusCode();
            _logger.LogInformation("Exiting GetUserName");
            return response;
        }
    }
}
