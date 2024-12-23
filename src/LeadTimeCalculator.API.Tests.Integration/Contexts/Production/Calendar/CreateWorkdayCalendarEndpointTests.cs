﻿using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.API.Tests.Integration.Contexts.Production.Calendar
{
    [Collection(LeadTimeCalculatorApiTestCollection.CollectionName)]
    public class CreateWorkdayCalendarEndpointTests
    {
        private readonly SutClient _sutClient;

        public CreateWorkdayCalendarEndpointTests(
            LeadTimeCalculatorAPIWebApplicationFactory fixture)
        {
            _sutClient = fixture.GetSutClient();
        }

        [Theory]
        [InlineData(8, 8)]
        [InlineData(16, 8)]
        [InlineData(0, 0)]
        public async Task InvalidRequest_ReturnsError(
            int startTimeAfterMidnight,
            int endTimeAfterMidnight)
        {
            // Given
            var validCreateCalendarRequest = new CreateWorkdayCalendarRequest(
                DefaultWorkdayStartTime: TimeSpan.FromHours(startTimeAfterMidnight),
                DefaultWorkdayEndTime: TimeSpan.FromHours(endTimeAfterMidnight));

            // When
            var createCalendarHttpResponse = await _sutClient
                .Production
                .Calendar
                .CreateWorkdayCalendar(validCreateCalendarRequest);

            // Then
            await Assert.AssertBadInputResponse(createCalendarHttpResponse);
        }

        [Fact]
        public async Task ValidRequest_CreatesCalendar()
        {
            // Given
            var validCreateCalendarRequest = new CreateWorkdayCalendarRequest(
                DefaultWorkdayStartTime: TimeSpan.FromHours(08),
                DefaultWorkdayEndTime: TimeSpan.FromHours(16));

            // When
            var createCalendarHttpResponse = await _sutClient
                .Production
                .Calendar
                .CreateWorkdayCalendar(validCreateCalendarRequest);

            var createCalendarResponse = await createCalendarHttpResponse
                .Content.ReadFromJsonAsync<CreateWorkdayCalendarResponse>();

            // Then
            await Assert.AssertSuccessfulResponse(createCalendarHttpResponse);

            var workdayCalendarsResponse = await GetWorkdayCalendars();
            Xunit.Assert.Contains(workdayCalendarsResponse.CalendarDetailedViews,
                x => x.Id == createCalendarResponse!.CalendarId);
        }

        private async Task<GetWorkdayCalendarsResponse> GetWorkdayCalendars()
        {
            var getWorkdayCalendarsHttpResponse = await _sutClient
                .Production
                .Calendar
                .GetWorkdayCalendars(new());
            getWorkdayCalendarsHttpResponse.EnsureSuccessStatusCode();

            var getWorkdayCalendarsResponse = await getWorkdayCalendarsHttpResponse
                .Content.ReadFromJsonAsync<GetWorkdayCalendarsResponse>();

            return getWorkdayCalendarsResponse!;
        }
    }
}
