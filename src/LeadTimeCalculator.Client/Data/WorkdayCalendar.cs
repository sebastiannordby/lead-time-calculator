using LeadTimeCalculator.Client.Data.Utilities;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateLeadTime;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.Client.Data
{
    public class WorkdayCalendar
    {
        private readonly HttpClient _httpClient;

        public WorkdayCalendar(
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

            var response = await httpReponse.ReadResponseAs<CalculateLeadTimeWorkdaysResponse>();

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
                await httpReponse.ReadResponseAs<GetWorkdayCalendarsResponse>();

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
                await httpResponse.ReadResponseAs<CreateWorkdayCalendarResponse>();

            return response;
        }
    }
}
