﻿using LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct;

namespace LeadTimeCalculator.API.Tests.Integration.Contexts.Production.Schedule
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class AddProducableProductEndpointTests
    {
        private readonly SutClient _sutClient;

        public AddProducableProductEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task Invalid_request_errors()
        {
            // Given
            var invalidRequest = new AddProducableProductRequest(
                ProductName: "",
                ProductType: "",
                WorkdaysToProduce: 0,
                Parts: []);

            // When
            var httpResponse = await _sutClient
                .Production
                .Schedule
                .AddProducableProduct(invalidRequest);

            // Then
            await Assert.AssertBadInputResponse(httpResponse);
        }
    }
}
