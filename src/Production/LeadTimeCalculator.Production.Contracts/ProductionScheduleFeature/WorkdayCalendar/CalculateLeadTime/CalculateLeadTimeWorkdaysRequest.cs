namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CalculateLeadTime
{
    public sealed record CalculateLeadTimeWorkdaysRequest(
        int CalendarId,
        DateTime StartingDate,
        double WorkdaysAdjustment);
}
