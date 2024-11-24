using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;
using LeadTimeCalculator.Client.Components.Dialogs;
using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace LeadTimeCalculator.Client.Components.Pages
{
    public partial class WorkdayCalendar
    {
        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        [Inject]
        public required DialogService DialogService { get; set; }

        private CalendarDetailedView? _selectedCalendar;
        private IEnumerable<CalendarDetailedView> _calendarViews = [];
        private IEnumerable<Appointment> _appointments = [];

        public string PageTitle
        {
            get => _selectedCalendar is not null
                ? $"{_selectedCalendar.Id} - Workday Calendar"
                : "Workday Calendar";
        }

        public async Task AddCalendar()
        {
            var createWorkdayCalendarResponse = await ApiClient.CreateWorkdayCalendarAsync(
                new CreateWorkdayCalendarRequest(
                    DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                    DefaultWorkdayEndTime: TimeSpan.FromHours(16)));
            var workdayCalendarId = createWorkdayCalendarResponse.CalendarId;

            await SetSelectedCalendar(workdayCalendarId);
        }

        public async Task LoadCalendars()
        {
            var getCalendarsResponse = await ApiClient
                .GetWorkdayCalendarsAsync(new());

            _calendarViews = getCalendarsResponse.CalendarDetailedViews;
        }

        public void OnSchedulerLoadData(SchedulerLoadDataEventArgs args)
        {
            UpdateAppointmentsForSelectedCalendar(
                args.Start,
                args.End);
        }

        public async Task SetSelectedCalendar(int? calendarId)
        {
            await LoadCalendars();

            _selectedCalendar = calendarId.HasValue ?
                _calendarViews.FirstOrDefault(x => x.Id == calendarId) : null;

            if (_selectedCalendar is not null)
            {
                UpdateAppointmentForSelectedCalendar();
            }
            else
            {
                _appointments = [];
            }

            await InvokeAsync(StateHasChanged);
        }

        private void UpdateAppointmentForSelectedCalendar()
        {
            var today = DateTime.Now;
            var rangeStart = new DateTime(today.Year, today.Month, 1);
            var rangeEnd = rangeStart.AddMonths(1);

            UpdateAppointmentsForSelectedCalendar(
                rangeStart, rangeEnd);
        }

        // Could have optimized this,
        // but the point of this exercise is not to prove 
        // my abilities to fully optimize a method in the frontend.
        private void UpdateAppointmentsForSelectedCalendar(
            DateTime rangeStartWorkingDays,
            DateTime rangeEndWorkingDays)
        {
            if (_selectedCalendar is null)
                return;

            var appointments = new List<Appointment>();

            var nonRecurringHolidayAsAppointments = _selectedCalendar.Holidays
                .Where(h => !h.IsRecurring && h.Date >= rangeStartWorkingDays.Date && h.Date <= rangeEndWorkingDays.Date)
                .Select(h => new Appointment
                {
                    Start = h.Date.Date,
                    End = h.Date.Date.AddDays(1).AddTicks(-1),
                    Text = "Holiday"
                });
            appointments.AddRange(nonRecurringHolidayAsAppointments);

            for (var date = rangeStartWorkingDays.Date; date < rangeEndWorkingDays.Date; date = date.AddDays(1))
            {
                foreach (var holiday in _selectedCalendar.Holidays.Where(h => h.IsRecurring))
                {
                    if (holiday.Date.Month == date.Month && holiday.Date.Day == date.Day)
                    {
                        appointments.Add(new Appointment
                        {
                            Start = date,
                            End = date.AddDays(1).AddTicks(-1),
                            Text = "Holiday (Recurring)"
                        });
                    }
                }
            }

            var exceptionDaysAsAppointment = _selectedCalendar.ExceptionDays
                .Select(x => new Appointment()
                {
                    Start = x.Date + x.StartTime,
                    End = x.Date + x.EndTime,
                    Text = "Exception Day"
                });
            appointments.AddRange(exceptionDaysAsAppointment);


            for (var date = rangeStartWorkingDays.Date; date < rangeEndWorkingDays.Date; date = date.AddDays(1))
            {
                if (_selectedCalendar.Holidays.Any(x => x.Date == date))
                    continue;

                if (exceptionDaysAsAppointment.Any(x => x.Start.Date == date))
                    continue;

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var defaultWorkHoursForDay = _selectedCalendar.DefaultWorkingDays
                    .First(x => x.DayOfWeek == date.DayOfWeek);

                appointments.Add(new Appointment
                {
                    Start = date.Add(defaultWorkHoursForDay.StartTime),
                    End = date.Add(defaultWorkHoursForDay.EndTime),
                    Text = "Working Day"
                });
            }

            _appointments = appointments;
        }

        public async Task ShowAddHolidayDialogAsync()
        {
            await DialogService.OpenAsync<AddWorkdayCalendarHolidayDialog>($"Add holiday",
                 new Dictionary<string, object>() { { "CalendarId", _selectedCalendar!.Id } },
                 new DialogOptions()
                 {
                     Resizable = false,
                     Draggable = true,
                     Width = "300px",
                     Height = "230px",
                 });

            await SetSelectedCalendar(_selectedCalendar.Id);
        }
    }
}
