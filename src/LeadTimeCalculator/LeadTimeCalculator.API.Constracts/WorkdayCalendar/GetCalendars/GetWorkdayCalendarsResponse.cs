namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars
{
    public sealed record GetWorkdayCalendarsResponse(
        IEnumerable<CalendarDetailedView> CalendarDetailedViews);
}
