namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiCollection.CollectionName)]
    public class CreateWorkdayCalendarEndpointTests
    {
        private readonly SutClient _sutClient;

        public CreateWorkdayCalendarEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory fixture)
        {
            _sutClient = fixture.GetSutClient();
        }

        [Theory]
        [InlineData(8, 8)]
        [InlineData(16, 8)]
        [InlineData(0, 0)]
        public async Task ShouldErrorOnInvalidInput(
            int startTimeAfterMidnight,
            int endTimeAfterMidnight)
        {
            var httpResponse = await _sutClient
                .CreateWorkdayCalendar(new(
                    DefaultWorkdayStartTime: TimeSpan.FromHours(startTimeAfterMidnight),
                    DefaultWorkdayEndTime: TimeSpan.FromHours(endTimeAfterMidnight)));

            Assert.AssertBadInputRequest(httpResponse);
        }
    }
}
