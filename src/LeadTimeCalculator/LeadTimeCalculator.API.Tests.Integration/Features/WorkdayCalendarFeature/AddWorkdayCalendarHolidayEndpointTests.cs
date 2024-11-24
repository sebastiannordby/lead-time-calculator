using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;

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
