using LeadTimeCalculator.API.Infrastructure.Repositories;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.Contracts;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.Production.Infrastructure.Queries
{
    /// <summary>
    /// This implementation should not use the repository to fetch these,
    /// but since this is an in memory implementation it "have" to.
    /// </summary>
    internal class InMemoryQueryCalendarDetailedView : IQueryCalendarDetailedView
    {
        private readonly InMemoryWorkdayCalendarRepository _workdayCalendarRepository;

        public InMemoryQueryCalendarDetailedView(
            InMemoryWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task<CalendarDetailedView[]> GetCalendarsAsync(
            CancellationToken cancellationToken)
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

                var workingDays = calendar.WorkWeek
                    .GetWorkingWeek()
                    .Select(day => new CalendarDetailedView.DefaultWorkingDayView()
                    {
                        DayOfWeek = day.Key,
                        EndTime = day.Value.EndTime,
                        StartTime = day.Value.StartTime
                    });

                return new CalendarDetailedView()
                {
                    Id = calendar.Id,
                    ExceptionDays = exceptionDays.ToArray(),
                    Holidays = holidays.ToArray(),
                    DefaultWorkingDays = workingDays.ToArray()
                };
            }).ToArray();

            return calendarDetailViews;
        }
    }
}
