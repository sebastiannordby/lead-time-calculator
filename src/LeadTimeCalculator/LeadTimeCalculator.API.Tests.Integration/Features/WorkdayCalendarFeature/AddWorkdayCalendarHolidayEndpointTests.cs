using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class AddWorkdayCalendarHolidayEndpointTests
    {
        private readonly SutClient _sutClient;

        public AddWorkdayCalendarHolidayEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task GivenInvalidRequest_ShouldError()
        {
            // Given
            var invalidRequest = new AddWorkdayCalendarHolidayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                IsRecurring: false);

            // When
            var addHolidayHttpResponse = await _sutClient
                .AddWorkdayCalendarHoliday(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(addHolidayHttpResponse);
        }

        [Fact]
        public async Task GivenValidRequest_ShouldAddExceptionDayToCalendar()
        {
            // Given
            var createCalendarRespose = await CreateWorkdayCalendar();

            var thisDate = DateTime.Now;

            var validAddExceptionDayRequest = new AddWorkdayCalendarHolidayRequest(
                CalendarId: createCalendarRespose!.CalendarId,
                Date: thisDate,
                IsRecurring: true);

            // When
            var addHolidayHttpResponse = await _sutClient
                .AddWorkdayCalendarHoliday(validAddExceptionDayRequest);

            // Then
            await Assert.AssertSuccessfulResponse(addHolidayHttpResponse);

            var getWorkdayCalendarsResponse = await GetWorkdayCalendars();

            Assert.Contains(getWorkdayCalendarsResponse.CalendarDetailedViews,
                calendar => calendar.Holidays.Any(holiday =>
                    holiday.Date.Date == thisDate.Date && holiday.IsRecurring));
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
