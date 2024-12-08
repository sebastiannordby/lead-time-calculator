using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace LeadTimeCalculator.Client.Components.Dialogs
{
    public partial class AddWorkdayCalendarExceptionDayDialog
    {
        [Parameter]
        public int CalendarId { get; set; }

        [Inject]
        public required DialogService DialogService { get; set; }

        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        private TimeSpan _startTime = TimeSpan.FromHours(8);
        private TimeSpan _endTime = TimeSpan.FromHours(16);
        private DateTime _date;
        private bool _isReccuring;

        public DateTime StartTime
        {
            get => new DateTime(
                _date.Year,
                _date.Month,
                _date.Day,
                _startTime.Hours,
                _startTime.Minutes,
                0);

            set
            {
                _startTime = value.TimeOfDay;
            }
        }

        public DateTime EndTime
        {
            get => new DateTime(
                _date.Year,
                _date.Month,
                _date.Day,
                _endTime.Hours,
                _endTime.Minutes,
                0);

            set
            {
                _endTime = value.TimeOfDay;
            }
        }

        private async Task ExecuteRequestAsync()
        {
            await ApiClient.WorkdayCalendar.AddWorkdayCalendarExceptionDayAsync(new(
                CalendarId: CalendarId,
                Date: _date,
                StartTime: _startTime,
                EndTime: _endTime));

            DialogService.Close();
        }
    }
}
