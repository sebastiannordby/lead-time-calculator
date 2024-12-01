using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace LeadTimeCalculator.Client.Components.Dialogs
{
    public partial class CreateWorkdayCalendarDialog
    {
        [Inject]
        public required DialogService DialogService { get; set; }

        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        private TimeSpan _startTime = TimeSpan.FromHours(8);
        private TimeSpan _endTime = TimeSpan.FromHours(16);
        private DateTime _date = DateTime.Now;
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
            var response = await ApiClient.CreateWorkdayCalendarAsync(new(
                DefaultWorkdayStartTime: _startTime,
                DefaultWorkdayEndTime: _endTime));

            DialogService.Close(response.CalendarId);
        }
    }
}
