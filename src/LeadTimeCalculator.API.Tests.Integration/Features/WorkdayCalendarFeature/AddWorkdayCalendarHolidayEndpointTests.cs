using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;

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
        public async Task InvalidRequest_ReturnsError()
        {
            // Given
            var invalidRequest = new AddWorkdayCalendarHolidayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                IsRecurring: false);

            // When
            var addHolidayHttpResponse = await _sutClient
                .WorkdayCalendar
                .AddWorkdayCalendarHoliday(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(addHolidayHttpResponse);
        }

        [Fact]
        public async Task ValidRequest_UpdatedCalendarWithHoliday()
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
                .WorkdayCalendar
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
