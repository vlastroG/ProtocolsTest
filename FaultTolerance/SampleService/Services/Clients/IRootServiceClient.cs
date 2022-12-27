using RootServiceNamespace;

namespace SampleService.Services.Clients
{
    public interface IRootServiceClient
    {
        public RootServiceNamespace.RootServiceClient RootServiceClient { get; }
        public Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
    }
}
