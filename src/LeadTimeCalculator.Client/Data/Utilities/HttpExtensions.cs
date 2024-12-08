namespace LeadTimeCalculator.Client.Data.Utilities
{
    internal static class HttpExtensions
    {
        internal static async Task<TResponse> ReadResponseAs<TResponse>(
            this HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            var deserializedResponse = await httpResponse.Content
                .ReadFromJsonAsync<TResponse>();

            return deserializedResponse!;
        }
    }
}
