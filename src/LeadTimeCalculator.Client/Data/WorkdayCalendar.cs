using LeadTimeCalculator.Client.Data.Utilities;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;
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

        public async Task<CalculateWorkdayCalendarTimeForwardResponse> CalculateWorkdayCalendarTimeForward(
            CalculateWorkdayCalendarTimeForwardRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/production/calendar/calculate-time-forward";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();

            var response = await httpReponse
                .ReadResponseAs<CalculateWorkdayCalendarTimeForwardResponse>();

            return response;
        }

        public async Task<CalculateWorkdayCalendarTimeBackwardResponse> CalculateWorkdayCalendarTimeBackward(
            CalculateWorkdayCalendarTimeBackwardRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/production/calendar/calculate-time-backward";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();

            var response = await httpReponse
                .ReadResponseAs<CalculateWorkdayCalendarTimeBackwardResponse>();

            return response;
        }

        public async Task AddWorkdayCalendarExceptionDayAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/production/calendar/add-exception-day";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();
        }

        public async Task AddWorkdayCalendarHolidayAsync(
            AddWorkdayCalendarHolidayRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/production/calendar/add-holiday";
            var httpReponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            httpReponse.EnsureSuccessStatusCode();
        }

        public async Task<GetWorkdayCalendarsResponse> GetWorkdayCalendarsAsync(
            GetWorkdayCalendarsRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = $"/api/production/calendar/list";
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
            var uri = "/api/production/calendar";
            var httpResponse = await _httpClient
                .PostAsJsonAsync(uri, request, cancellationToken);
            var response =
                await httpResponse.ReadResponseAs<CreateWorkdayCalendarResponse>();

            return response;
        }
    }
}
