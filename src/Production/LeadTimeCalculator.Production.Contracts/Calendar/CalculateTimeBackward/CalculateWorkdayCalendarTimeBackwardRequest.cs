namespace LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward
{
    public sealed record CalculateWorkdayCalendarTimeBackwardRequest(
        int WorkdayCalendarId,
        DateTime DateTimeToSubtractFrom,
        double WorkdaysToSubtract
    );
}
