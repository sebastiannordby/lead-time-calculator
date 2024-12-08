namespace LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward
{
    public sealed record CalculateWorkdayCalendarTimeBackwardResponse(
        DateTime StartDateTime,
        DateTime EndDateTime,
        double WorkdaysSubtracted
    );
}
