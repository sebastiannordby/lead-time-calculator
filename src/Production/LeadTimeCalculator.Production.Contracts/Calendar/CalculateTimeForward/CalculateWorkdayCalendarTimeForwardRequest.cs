namespace LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward
{
    public sealed record CalculateWorkdayCalendarTimeForwardRequest(
        int WorkdayCalendarId,
        DateTime StartDateTime,
        double WorkdaysToAdd
    );
}
