namespace LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars
{
    public sealed record GetWorkdayCalendarsResponse(
        IEnumerable<CalendarDetailedView> CalendarDetailedViews);
}
