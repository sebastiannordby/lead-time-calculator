namespace LeadTimeCalculator.Production.Contracts.Calendar.CalculateLeadTime
{
    public sealed record CalculateLeadTimeWorkdaysRequest(
        int CalendarId,
        DateTime StartingDate,
        double WorkdaysAdjustment);
}
