using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateLeadTime;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Shared
{
    internal class WorkdayCalendarEndpoints
    {
        private readonly HttpClient _httpClient;

        internal WorkdayCalendarEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task<HttpResponseMessage> CalculateLeadTimeWorkdays(
            CalculateLeadTimeWorkdaysRequest request)
        {
            var uri = "/api/workday-calendar/calculate-lead-time-workdays";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> AddWorkdayCalendarExceptionDay(
            AddWorkdayCalendarExceptionDayRequest request)
        {
            var uri = "/api/workday-calendar/add-exception-day";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> AddWorkdayCalendarHoliday(
            AddWorkdayCalendarHolidayRequest request)
        {
            var uri = "/api/workday-calendar/add-holiday";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> CreateWorkdayCalendar(
            CreateWorkdayCalendarRequest request)
        {
            var uri = "/api/workday-calendar";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> GetWorkdayCalendars(
            GetWorkdayCalendarsRequest request)
        {
            var uri = $"/api/workday-calendar/list";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }
    }
}
