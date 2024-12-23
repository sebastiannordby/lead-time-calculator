﻿using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Contexts.Production.Calendar
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class GetWorkdayCalendarsEndpointTests
    {
        private readonly SutClient _sutClient;

        public GetWorkdayCalendarsEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory factory)
        {
            _sutClient = factory.GetSutClient();
        }

        [Fact]
        public async Task FetchesStoredCalendars()
        {
            // Given
            var createCalendarId = await CreateWorkdayCalendar();

            // When
            var getWorkdayCalendarsHttpResponse = await _sutClient
                .Production
                .Calendar
                .GetWorkdayCalendars(new());

            // Then
            await Assert.AssertSuccessfulResponse(getWorkdayCalendarsHttpResponse);

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            Xunit.Assert.Contains(getWorkdayCalendarsResponse!.CalendarDetailedViews!,
                x => x.Id == createCalendarId);
        }

        private async Task<int> CreateWorkdayCalendar()
        {
            var createCalendarHttpResponse = await _sutClient
                .Production
                .Calendar
                .CreateWorkdayCalendar(
                    new CreateWorkdayCalendarRequest(
                        DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                        DefaultWorkdayEndTime: TimeSpan.FromHours(16)));
            createCalendarHttpResponse.EnsureSuccessStatusCode();

            var createCalendarResponse = await createCalendarHttpResponse.Content
                .ReadFromJsonAsync<CreateWorkdayCalendarResponse>();

            return createCalendarResponse!.CalendarId;
        }
    }
}
