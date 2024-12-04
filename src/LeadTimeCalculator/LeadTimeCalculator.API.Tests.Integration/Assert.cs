using System.Net;

namespace LeadTimeCalculator.API.Tests.Integration
{
    public partial class Assert : Xunit.Assert
    {
        public static async Task AssertSuccessfulResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                await ThrowRequestError(response);
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static async Task AssertBadInputResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                await ThrowRequestError(response);
            }

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static async Task ThrowRequestError(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var displayContent = !string.IsNullOrWhiteSpace(content)
                ? content
                : "EMPTY CONTENT";

            throw new Exception($"{response.RequestMessage?.Method} : {response.StatusCode} : {displayContent} : {response.RequestMessage?.RequestUri}");
        }
    }
}
