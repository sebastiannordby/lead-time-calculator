namespace LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward
{
    public sealed record CalculateWorkdayCalendarTimeForwardResponse(
        DateTime StartDateTime,
        DateTime EndDateTime,
        double WorkdaysAdded
    );
}
