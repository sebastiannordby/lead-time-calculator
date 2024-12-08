using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Shared
{
    internal class ProductionWorkdayCalendarEndpoints
    {
        private readonly HttpClient _httpClient;

        internal ProductionWorkdayCalendarEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task<HttpResponseMessage> CalculateWorkdayCalendarTimeBackward(
            CalculateWorkdayCalendarTimeBackwardRequest request)
        {
            var uri = "/api/production/calendar/calculate-time-backward";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> AddWorkdayCalendarExceptionDay(
            AddWorkdayCalendarExceptionDayRequest request)
        {
            var uri = "/api/production/calendar/add-exception-day";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> AddWorkdayCalendarHoliday(
            AddWorkdayCalendarHolidayRequest request)
        {
            var uri = "/api/production/calendar/add-holiday";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> CreateWorkdayCalendar(
            CreateWorkdayCalendarRequest request)
        {
            var uri = "/api/production/calendar";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> GetWorkdayCalendars(
            GetWorkdayCalendarsRequest request)
        {
            var uri = $"/api/production/calendar/list";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }

        internal async Task<HttpResponseMessage> CalculateWorkdayCalendarTimeForward(
            CalculateWorkdayCalendarTimeForwardRequest request)
        {
            var uri = $"/api/production/calendar/calculate-time-forward";
            var response = await _httpClient
                .PostAsJsonAsync(uri, request);

            return response;
        }
    }
}
