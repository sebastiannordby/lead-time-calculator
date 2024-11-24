using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;

namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class CalculateLeadTimeWorkdaysEndpointTests
    {
        private readonly SutClient _sutClient;

        public CalculateLeadTimeWorkdaysEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task ShouldValidateRequest()
        {
            // Arrange
            var invalidRequest = new CalculateLeadTimeWorkdaysRequest(
                CalendarId: 0,
                StartingDate: DateTime.MinValue,
                WorkdaysAdjustment: 0);

            // Act
            var calculateLeadTimeHttpResponse = await _sutClient
                .CalculateLeadTimeWorkdays(invalidRequest);

            // Assert
            Assert.AssertBadInputResponse(calculateLeadTimeHttpResponse);
        }
    }
}
