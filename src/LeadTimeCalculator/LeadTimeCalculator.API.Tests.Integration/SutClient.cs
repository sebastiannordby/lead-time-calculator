using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public class SutClient
    {
        private readonly HttpClient _httpClient;

        public SutClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
