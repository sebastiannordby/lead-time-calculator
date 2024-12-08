using LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct;

namespace LeadTimeCalculator.API.Tests.Integration.Shared
{
    internal class ProductionScheduleEndpoints
    {
        private readonly HttpClient _httpClient;

        internal ProductionScheduleEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task<HttpResponseMessage> AddProducableProduct(
            AddProducableProductRequest request)
        {
            var uri = "/api/production/schedule/producable-product";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }
    }
}
