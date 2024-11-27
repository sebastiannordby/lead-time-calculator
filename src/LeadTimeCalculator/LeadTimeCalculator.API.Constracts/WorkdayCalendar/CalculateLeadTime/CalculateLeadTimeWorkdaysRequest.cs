
namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime
{
    public sealed record CalculateLeadTimeWorkdaysRequest(
        int CalendarId,
        DateTime StartingDate,
        double WorkdaysAdjustment);
}
