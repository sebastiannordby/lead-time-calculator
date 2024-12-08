using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;

namespace LeadTimeCalculator.API.Tests.Integration.Contexts.Production.Calendar
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class CalculateWorkdayCalendarTimeBackwardEndpointTests
    {
        private readonly SutClient _sutClient;

        public CalculateWorkdayCalendarTimeBackwardEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task Errors_on_invalid_request()
        {
            // Given
            var invalidRequest = new CalculateWorkdayCalendarTimeBackwardRequest(
                WorkdayCalendarId: 0,
                DateTimeToSubtractFrom: DateTime.MinValue,
                WorkdaysToSubtract: 0);

            // When
            var httpResponse = await _sutClient
                .Production
                .Calendar
                .CalculateWorkdayCalendarTimeBackward(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(httpResponse);
        }
    }
}
