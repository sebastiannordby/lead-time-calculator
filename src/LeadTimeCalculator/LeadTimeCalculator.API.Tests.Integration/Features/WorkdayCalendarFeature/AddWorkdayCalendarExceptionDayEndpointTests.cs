using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;

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
        public async Task ShouldValidateRequest()
        {
            // Arrange
            var invalidRequest = new AddWorkdayCalendarExceptionDayRequest(
                CalendarId: 0,
                Date: DateTime.MinValue,
                StartTime: TimeSpan.Zero,
                EndTime: TimeSpan.Zero);

            // Act
            var addWorkdayHttpResponse = await _sutClient
                .AddWorkdayCalendarExceptionDay(invalidRequest);

            // Assert
            Assert.AssertBadInputResponse(addWorkdayHttpResponse);
        }
    }
}
