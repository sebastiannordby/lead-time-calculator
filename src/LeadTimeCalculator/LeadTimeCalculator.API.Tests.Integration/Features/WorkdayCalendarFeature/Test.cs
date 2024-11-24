namespace LeadTimeCalculator.API.Tests.Integration.Features.WorkdayCalendarFeature
{
    [Collection(LeadTimeCalculatorApiCollection.CollectionName)]
    public class Test
    {
        private readonly SutClient _sutClient;

        public Test(LeadTimeCalculatorAPIWebApplicationFactory fixture)
        {
            _sutClient = fixture.GetSutClient();
        }

        [Fact]
        public async Task ShouldReturnValues()
        {
            var weatherForecastResponse = await _sutClient.GetWeatherForecast();

            Assert.AssertSuccessfulRequest(weatherForecastResponse);
        }
    }
}
