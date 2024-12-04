namespace LeadTimeCalculator.Client.Data
{
    public class LeadTimeApiClient
    {
        private readonly HttpClient _httpClient;
        public WorkdayCalendar WorkdayCalendar { get; }

        public LeadTimeApiClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            WorkdayCalendar = new(_httpClient);
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
