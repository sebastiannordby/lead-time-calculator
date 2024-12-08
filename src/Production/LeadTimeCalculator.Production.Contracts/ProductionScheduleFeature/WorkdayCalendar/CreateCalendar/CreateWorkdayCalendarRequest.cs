namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar
{
    public sealed record CreateWorkdayCalendarRequest(
        TimeSpan DefaultWorkdayStartTime,
        TimeSpan DefaultWorkdayEndTime);
}
