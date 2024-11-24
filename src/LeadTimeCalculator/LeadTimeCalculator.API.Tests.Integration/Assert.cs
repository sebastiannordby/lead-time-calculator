using System.Net;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public partial class Assert
    {
        public static void AssertSuccessfulRequest(HttpResponseMessage response)
        {
            Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static void AssertBadInputRequest(HttpResponseMessage response)
        {
            Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
