using System.Net;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public partial class Assert : Xunit.Assert
    {
        public static void AssertSuccessfulResponse(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static void AssertBadInputResponse(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
