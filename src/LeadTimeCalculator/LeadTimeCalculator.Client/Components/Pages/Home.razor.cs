using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;

namespace LeadTimeCalculator.Client.Components.Pages
{
    public partial class Home
    {
        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        public async Task AddSchedule()
        {
            var createWorkdayCalendarResponse = await ApiClient.CreateWorkdayCalendarAsync(
                new CreateWorkdayCalendarRequest(
                    DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                    DefaultWorkdayEndTime: TimeSpan.FromHours(16)));
            var workdayCalendarId = createWorkdayCalendarResponse.CalendarId;
        }
    }
}
