namespace LeadTimeCalculator.API.Tests.Integration
{
    [CollectionDefinition(CollectionName)]
    public class LeadTimeCalculatorApiTestCollection : ICollectionFixture<LeadTimeCalculatorAPIWebApplicationFactory>
    {
        public const string CollectionName = nameof(LeadTimeCalculatorApiTestCollection);
    }
}
