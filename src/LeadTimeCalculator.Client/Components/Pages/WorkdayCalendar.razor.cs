using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;
using LeadTimeCalculator.Client.Components.Dialogs;
using LeadTimeCalculator.Client.Data;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace LeadTimeCalculator.Client.Components.Pages
{
    [Route(LeadTimeCalculatorRoutes.WORKDAY_CALENDAR)]
    public partial class WorkdayCalendar
    {
        [Inject]
        public required LeadTimeApiClient ApiClient { get; set; }

        [Inject]
        public required DialogService DialogService { get; set; }

        private CalendarDetailedView? _selectedCalendar;
        private IEnumerable<CalendarDetailedView> _calendarViews = [];
        private IEnumerable<Appointment> _appointments = [];
        private RadzenScheduler<Appointment>? _scheduler;

        private DateTime _startProductionDate;
        private double _startProductionDateWorkdaysToComplete;

        private DateTime _requestedShippingDate;
        private double _requestShippingDateWorkdaysToComplete;

        public string PageTitle
        {
            get => _selectedCalendar is not null
                ? $"{_selectedCalendar.Id} - Workday Calendar"
                : "Workday Calendar";
        }

        public async Task AddCalendar()
        {
            var workdayCalendarId = await DialogService.OpenAsync<CreateWorkdayCalendarDialog>($"Create calendar",
                 new Dictionary<string, object>() { },
                 new DialogOptions()
                 {
                     Resizable = false,
                     Draggable = true,
                     Width = "300px",
                     Height = "330px",
                 });

            await SetSelectedCalendar(workdayCalendarId);
        }

        private async Task CalculateShippingDate()
        {
            var calculateLeadTimeResponse = await ApiClient
                .WorkdayCalendar
                .CalculateLeadTimeWorkdaysResponse(new(
                    CalendarId: _selectedCalendar!.Id,
                    StartingDate: _startProductionDate,
                    WorkdaysAdjustment: _startProductionDateWorkdaysToComplete));

            var leadTimeMessage = $@"If you start production at {_startProductionDate}
                and it takes {_startProductionDateWorkdaysToComplete} workdays to produce,
                you will be able to ship the product at {calculateLeadTimeResponse.StartOrEndTime}";

            await DialogService.Alert(leadTimeMessage, "Lead Time");
        }

        private async Task CalculateProductionStartDateForShipping()
        {
            var calculateLeadTimeResponse = await ApiClient
                .WorkdayCalendar
                .CalculateLeadTimeWorkdaysResponse(new(
                    CalendarId: _selectedCalendar!.Id,
                    StartingDate: _requestedShippingDate,
                    WorkdaysAdjustment: -1 * _requestShippingDateWorkdaysToComplete));

            var leadTimeMessage = $@"If you want to ship at {_requestedShippingDate}
                and it takes {_requestShippingDateWorkdaysToComplete} workdays to produce,
                you will have to start production latest at {calculateLeadTimeResponse.StartOrEndTime}";

            await DialogService.Alert(leadTimeMessage, "Lead Time");
        }

        public async Task LoadCalendars()
        {
            var getCalendarsResponse = await ApiClient
                .WorkdayCalendar
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
                if (_scheduler is null)
                {
                    UpdateAppointmentForSelectedCalendar();
                }
                else
                {
                    await _scheduler.Reload();
                }
            }
            else
            {
                _appointments = [];
            }
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
                    Text = $"Exception Day ({x.StartTime}-{x.EndTime})"
                });
            appointments.AddRange(exceptionDaysAsAppointment);


            for (var date = rangeStartWorkingDays.Date; date < rangeEndWorkingDays.Date; date = date.AddDays(1))
            {
                var isHoliday = _selectedCalendar.Holidays.Any(h => h.Date == date ||
                    (h.IsRecurring && h.Date.Month == date.Month && h.Date.Day == date.Day));
                if (isHoliday)
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
                    Text = $"Working Day ({defaultWorkHoursForDay.StartTime}-{defaultWorkHoursForDay.EndTime})"
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

        public async Task ShowAddExceptionDayDialogAsync()
        {
            await DialogService.OpenAsync<AddWorkdayCalendarExceptionDayDialog>($"Add exception day",
                 new Dictionary<string, object>() { { "CalendarId", _selectedCalendar!.Id } },
                 new DialogOptions()
                 {
                     Resizable = false,
                     Draggable = true,
                     Width = "300px",
                     Height = "330px",
                 });

            await SetSelectedCalendar(_selectedCalendar.Id);
        }
    }
}
