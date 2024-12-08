using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;

namespace LeadTimeCalculator.API.Tests.Integration.Contexts.Production.Calendar
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class CalculateWorkdayCalendarTimeForwardEndpointTests
    {
        private readonly SutClient _sutClient;

        public CalculateWorkdayCalendarTimeForwardEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task Errors_on_invalid_request()
        {
            // Given
            var invalidRequest = new CalculateWorkdayCalendarTimeForwardRequest(
                WorkdayCalendarId: 0,
                StartDateTime: DateTime.MinValue,
                WorkdaysToAdd: 0);

            // When
            var httpResponse = await _sutClient
                .Production
                .Calendar
                .CalculateWorkdayCalendarTimeForward(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(httpResponse);
        }
    }
}
