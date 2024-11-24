using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;

namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiCollection.CollectionName)]
    public class AddWorkdayCalendarHolidatEndpointTests
    {
        private readonly SutClient _sutClient;

        public AddWorkdayCalendarHolidatEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task ShouldValidateRequest()
        {
            // Arrange
            var invalidRequest = new AddWorkdayCalendarHolidayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                IsRecurring: false);

            // Act
            var addHolidayHttpResponse = await _sutClient
                .AddWorkdayCalendarHoliday(invalidRequest);

            // Assert
            Assert.AssertBadInputResponse(addHolidayHttpResponse);
        }
    }
}
