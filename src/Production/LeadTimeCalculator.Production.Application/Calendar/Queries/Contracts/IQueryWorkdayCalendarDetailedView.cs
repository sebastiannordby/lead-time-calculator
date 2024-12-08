using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.Calendar.Queries.Contracts
{
    public interface IQueryWorkdayCalendarDetailedView
    {
        Task<CalendarDetailedView[]> GetCalendarsAsync(
            CancellationToken cancellationToken);
    }
}
