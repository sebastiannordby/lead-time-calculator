namespace LeadTimeCalculator.API.Tests.Integration
{
    public class SutClient
    {
        private readonly HttpClient _httpClient;

        public SutClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task<HttpResponseMessage> GetWeatherForecast()
        {
            var response = await _httpClient.GetAsync("/weatherforecast");

            return response;
        }
    }
}
