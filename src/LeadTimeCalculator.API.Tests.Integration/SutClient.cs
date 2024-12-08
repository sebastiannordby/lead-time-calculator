namespace LeadTimeCalculator.API.Tests.Integration
{
    internal class SutClient
    {
        internal ProductionEndpoints Production { get; }
        private readonly HttpClient _httpClient;

        internal SutClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Production = new(httpClient);
        }
    }
}
