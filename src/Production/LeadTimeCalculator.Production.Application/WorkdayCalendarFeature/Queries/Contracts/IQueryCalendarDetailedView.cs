using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.Contracts
{
    public interface IQueryCalendarDetailedView
    {
        Task<CalendarDetailedView[]> GetCalendarsAsync(
            CancellationToken cancellationToken);
    }
}
