namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar
{
    public sealed record CreateWorkdayCalendarRequest(
        TimeSpan DefaultWorkdayStartTime,
        TimeSpan DefaultWorkdayEndTime);
}
