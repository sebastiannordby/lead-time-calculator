using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

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
        public async Task GivenInvalidRequest_ShouldError()
        {
            // Given
            var invalidRequest = new AddWorkdayCalendarExceptionDayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                StartTime: TimeSpan.Zero,
                EndTime: TimeSpan.Zero);

            // When
            var addWorkdayHttpResponse = await _sutClient
                .AddWorkdayCalendarExceptionDay(invalidRequest);

            // Then
            Assert.AssertBadInputResponse(addWorkdayHttpResponse);
        }

        [Fact]
        public async Task GivenValidRequest_ShouldAddExceptionDayToCalendar()
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
                .AddWorkdayCalendarExceptionDay(validAddExceptionDayRequest);

            // Then
            Assert.AssertSuccessfulResponse(addExceptionDayHttpResponse);

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
                .GetWorkdayCalendars(new());
            getWorkdayCalendarsHttpResponse.EnsureSuccessStatusCode();

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            return getWorkdayCalendarsResponse!;
        }

        private async Task<CreateWorkdayCalendarResponse> CreateWorkdayCalendar()
        {
            var createCalendarHttpResponse = await _sutClient
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
