using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

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

        public async Task AddWorkdayCalendarExceptionDayAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/workday-calendar/add-exception-day";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();
        }

        public async Task AddWorkdayCalendarHolidayAsync(
            AddWorkdayCalendarHolidayRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/workday-calendar/add-holiday";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();
        }

        public async Task<GetWorkdayCalendarsResponse> GetWorkdayCalendarsAsync(
            GetWorkdayCalendarsRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/workday-calendar/list";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            var response =
                await ReadResponseAs<GetWorkdayCalendarsResponse>(httpReponse);

            return response;
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
