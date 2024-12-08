using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;
using LeadTimeCalculator.Client.Data.Utilities;

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
