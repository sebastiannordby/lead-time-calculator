namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddExceptionDay
{
    public sealed record AddWorkdayCalendarExceptionDayRequest(
        int CalendarId,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime);
}
