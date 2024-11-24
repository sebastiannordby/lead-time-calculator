using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases
{
    internal sealed class GetWorkdayCalendarsRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public GetWorkdayCalendarsRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        internal async Task<GetWorkdayCalendarsResponse> HandleAsync(
            GetWorkdayCalendarsRequest request,
            CancellationToken cancellationToken = default)
        {
            var calendars = await _workdayCalendarRepository
                .GetAllAsync(cancellationToken);
            var calendarDetailViews = calendars.Select(calendar =>
            {
                var exceptionDays = calendar.ExceptionDays
                    .Select(exceptionDay => new CalendarDetailedView.ExceptionDayView()
                    {
                        Date = exceptionDay.Date,
                        EndTime = exceptionDay.EndTime,
                        StartTime = exceptionDay.StartTime
                    });

                var holidays = calendar.Holidays
                    .Select(holiday => new CalendarDetailedView.HolidayView()
                    {
                        Date = holiday.Date,
                        IsRecurring = holiday.IsRecurring
                    });

                var workingDays = calendar.DefaultWorkHours
                    .Select(day => new CalendarDetailedView.DefaultWorkingDayView()
                    {
                        DayOfWeek = day.Key,
                        EndTime = day.Value.End,
                        StartTime = day.Value.Start
                    });

                return new CalendarDetailedView()
                {
                    Id = calendar.Id,
                    ExceptionDays = exceptionDays.ToArray(),
                    Holidays = holidays.ToArray(),
                    DefaultWorkingDays = workingDays.ToArray()
                };
            });

            var response = new GetWorkdayCalendarsResponse(calendarDetailViews);

            return response;
        }
    }
}
