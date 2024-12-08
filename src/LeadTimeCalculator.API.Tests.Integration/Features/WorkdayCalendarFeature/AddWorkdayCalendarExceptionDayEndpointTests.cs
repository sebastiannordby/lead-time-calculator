using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class AddWorkdayCalendarExceptionDayEndpointTests
    {
        private readonly SutClient _sutClient;

        public AddWorkdayCalendarExceptionDayEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task InvalidRequest_ReturnsError()
        {
            // Given
            var invalidRequest = new AddWorkdayCalendarExceptionDayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                StartTime: TimeSpan.Zero,
                EndTime: TimeSpan.Zero);

            // When
            var addWorkdayHttpResponse = await _sutClient
                .WorkdayCalendar
                .AddWorkdayCalendarExceptionDay(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(addWorkdayHttpResponse);
        }

        [Fact]
        public async Task ValidRequest_UpdatesCalendarWithExceptionDay()
        {
            // Given
            var createCalendarRespose = await CreateWorkdayCalendar();

            var thisDate = DateTime.Now;

            var validAddExceptionDayRequest = new AddWorkdayCalendarExceptionDayRequest(
                CalendarId: createCalendarRespose!.CalendarId,
                Date: thisDate,
                StartTime: TimeSpan.FromHours(12),
                EndTime: TimeSpan.FromHours(14));

            // When
            var addExceptionDayHttpResponse = await _sutClient
                .WorkdayCalendar
                .AddWorkdayCalendarExceptionDay(validAddExceptionDayRequest);

            // Then
            await Assert.AssertSuccessfulResponse(addExceptionDayHttpResponse);

            var getWorkdayCalendarsResponse = await GetWorkdayCalendars();

            Assert.Contains(getWorkdayCalendarsResponse.CalendarDetailedViews,
                calendar => calendar.ExceptionDays.Any(exDay =>
                    exDay.Date.Date == thisDate.Date
                    && exDay.StartTime.Hours == 12
                    && exDay.EndTime.Hours == 14));
        }

        private async Task<GetWorkdayCalendarsResponse> GetWorkdayCalendars()
        {
            var getWorkdayCalendarsHttpResponse = await _sutClient
                .WorkdayCalendar
                .GetWorkdayCalendars(new());
            getWorkdayCalendarsHttpResponse.EnsureSuccessStatusCode();

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            return getWorkdayCalendarsResponse!;
        }

        private async Task<CreateWorkdayCalendarResponse> CreateWorkdayCalendar()
        {
            var createCalendarHttpResponse = await _sutClient
                .WorkdayCalendar
                .CreateWorkdayCalendar(
                    new CreateWorkdayCalendarRequest(
                        TimeSpan.FromHours(8),
                        TimeSpan.FromHours(16)));
            createCalendarHttpResponse.EnsureSuccessStatusCode();

            var createCalendarRespose = await createCalendarHttpResponse
                .Content.ReadFromJsonAsync<CreateWorkdayCalendarResponse>();

            return createCalendarRespose!;
        }
    }
}
