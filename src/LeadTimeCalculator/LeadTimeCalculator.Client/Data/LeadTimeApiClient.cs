using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;

namespace LeadTimeCalculator.Client.Data
{
    public class LeadTimeApiClient
    {
        private readonly HttpClient _httpClient;

        public LeadTimeApiClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateWorkdayCalendarResponse> CreateWorkdayCalendarAsync(
            CreateWorkdayCalendarRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = "/api/workday-calendar";
            var httpResponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            var response =
                await ReadResponseAs<CreateWorkdayCalendarResponse>(httpResponse);

            return response;
        }

        public async Task<TResponse> ReadResponseAs<TResponse>(
            HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            var deserializedResponse = await httpResponse.Content
                .ReadFromJsonAsync<TResponse>();

            return deserializedResponse!;
        }
    }
}
