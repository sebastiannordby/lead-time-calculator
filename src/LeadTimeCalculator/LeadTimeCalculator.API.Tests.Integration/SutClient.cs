using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration
{
    /// <summary>
    /// Notes:
    /// Would ideally been grouped in classes which is related to the feature
    /// instead of repeating WorkdayCalendar, context matters though, and this approach is fine for this exercise.
    /// 
    /// Example:
    /// WorkdayCalendarEndpoints:
    /// - AddWorkday
    /// - CreateCalendar
    /// - GetCalendars
    /// </summary>
    internal class SutClient
    {
        private readonly HttpClient _httpClient;

        internal SutClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
