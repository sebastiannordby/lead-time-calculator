using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.Client.Data
{
    /// <summary>
    /// Notes:
    /// Would ideally been grouped in classes which is related to the feature
    /// instead of repeating WorkdayCalendar, context matters though, and this approach is fine for this exercise. 
    /// 
    /// Example:
    /// WorkdayCalendar:
    /// - AddWorkday
    /// - CreateCalendar
    /// - GetCalendars
    /// </summary>

    public class LeadTimeApiClient
    {
        private readonly HttpClient _httpClient;

        public LeadTimeApiClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CalculateLeadTimeWorkdaysResponse> CalculateLeadTimeWorkdaysResponse(
            CalculateLeadTimeWorkdaysRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/workday-calendar/calculate-lead-time-workdays";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();

            var response = await ReadResponseAs<CalculateLeadTimeWorkdaysResponse>(httpReponse);

            return response;

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
