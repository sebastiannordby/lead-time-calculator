using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class GetWorkdayCalendarsEndpointTests
    {
        private readonly SutClient _sutClient;

        public GetWorkdayCalendarsEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task FetchesStoredCalendars()
        {
            // Given
            var createCalendarId = await CreateWorkdayCalendar();

            // When
            var getWorkdayCalendarsHttpResponse = await _sutClient
                .GetWorkdayCalendars(new());

            // Then
            await Assert.AssertSuccessfulResponse(getWorkdayCalendarsHttpResponse);

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            Assert.Contains(getWorkdayCalendarsResponse!.CalendarDetailedViews!,
                x => x.Id == createCalendarId);
        }

        private async Task<int> CreateWorkdayCalendar()
        {
            var createCalendarHttpResponse = await _sutClient.CreateWorkdayCalendar(
                new CreateWorkdayCalendarRequest(
                    DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                    DefaultWorkdayEndTime: TimeSpan.FromHours(16)));
            createCalendarHttpResponse.EnsureSuccessStatusCode();

            var createCalendarResponse = await createCalendarHttpResponse.Content
                .ReadFromJsonAsync<CreateWorkdayCalendarResponse>();

            return createCalendarResponse!.CalendarId;
        }
    }
}
