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
        public async Task ShouldReturnValues()
        {
            // Arrange
            var createCalendarHttpResponse = await _sutClient.CreateWorkdayCalendar(
                new CreateWorkdayCalendarRequest(
                    DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                    DefaultWorkdayEndTime: TimeSpan.FromHours(16)));
            createCalendarHttpResponse.EnsureSuccessStatusCode();
            var createCalendarRespnse = await createCalendarHttpResponse.Content
                .ReadFromJsonAsync<CreateWorkdayCalendarResponse>();

            // Act
            var getWorkdayCalendarsHttpResponse = await _sutClient
                .GetWorkdayCalendars(new());

            // Assert
            Assert.AssertSuccessfulResponse(getWorkdayCalendarsHttpResponse);

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            Assert.Contains(getWorkdayCalendarsResponse!.CalendarDetailedViews!,
                x => x.Id == createCalendarRespnse!.CalendarId);
        }
    }
}
