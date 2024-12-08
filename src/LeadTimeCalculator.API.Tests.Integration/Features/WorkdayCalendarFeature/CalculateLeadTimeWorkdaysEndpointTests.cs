using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CalculateLeadTime;

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
        public async Task InvalidRequest_ReturnsError()
        {
            // Given
            var invalidRequest = new CalculateLeadTimeWorkdaysRequest(
                CalendarId: 0,
                StartingDate: DateTime.MinValue,
                WorkdaysAdjustment: 0);

            // When
            var calculateLeadTimeHttpResponse = await _sutClient
                .WorkdayCalendar
                .CalculateLeadTimeWorkdays(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(calculateLeadTimeHttpResponse);
        }
    }
}
