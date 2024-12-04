using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace LeadTimeCalculator.Client.Components.Dialogs
{
    public partial class AddWorkdayCalendarHolidayDialog
    {
        [Parameter]
        public int CalendarId { get; set; }

        [Inject]
        public required DialogService DialogService { get; set; }

        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        private DateTime _holidayDate;
        private bool _isReccuring;

        private async Task ExecuteRequestAsync()
        {
            await ApiClient.WorkdayCalendar.AddWorkdayCalendarHolidayAsync(new(
                CalendarId: CalendarId,
                Date: _holidayDate,
                IsRecurring: _isReccuring));

            DialogService.Close();
        }
    }
}
