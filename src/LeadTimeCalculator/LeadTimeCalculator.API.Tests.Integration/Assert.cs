using System.Net;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public partial class Assert : Xunit.Assert
    {
        public static async Task AssertSuccessfulResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static async Task AssertBadInputResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
