namespace LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar
{
    public sealed record CreateWorkdayCalendarRequest(
        TimeSpan DefaultWorkdayStartTime,
        TimeSpan DefaultWorkdayEndTime);
}
