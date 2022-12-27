using RootServiceNamespace;

namespace SampleService.Services.Clients.Impl
{
    public class RootServiceClient : IRootServiceClient
    {
        private readonly ILogger<RootServiceClient> _logger;
        private readonly RootServiceNamespace.RootServiceClient _rootServiceClient;

        public RootServiceClient(
            ILogger<RootServiceClient> logger,
            HttpClient httpClient)
        {
            _logger = logger;
            _rootServiceClient = new RootServiceNamespace.RootServiceClient("http://localhost:5167",
                httpClient);
        }
        RootServiceNamespace.RootServiceClient IRootServiceClient.RootServiceClient => _rootServiceClient;

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
        {
            return await _rootServiceClient.GetWeatherForecastAsync();
        }
    }
}
