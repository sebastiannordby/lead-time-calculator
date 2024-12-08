namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars
{
    public sealed record GetWorkdayCalendarsResponse(
        IEnumerable<CalendarDetailedView> CalendarDetailedViews);
}
