using Microsoft.AspNetCore.Mvc.Testing;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public class LeadTimeCalculatorAPIWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        private SutClient _sutClient = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => { });
            builder.UseEnvironment("Development");
        }

        internal SutClient GetSutClient()
        {
            if (_sutClient is null)
            {
                var httpClient = CreateClient();

                _sutClient = new(httpClient);
            }

            return _sutClient;
        }
    }
}
