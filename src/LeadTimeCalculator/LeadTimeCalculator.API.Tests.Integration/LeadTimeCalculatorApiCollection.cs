namespace LeadTimeCalculator.API.Tests.Integration
{
    [CollectionDefinition(CollectionName)]
    public class LeadTimeCalculatorApiCollection : ICollectionFixture<LeadTimeCalculatorAPIWebApplicationFactory>
    {
        public const string CollectionName = nameof(LeadTimeCalculatorApiCollection);
    }
}
