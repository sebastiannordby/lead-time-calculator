namespace LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay
{
    public sealed record AddWorkdayCalendarExceptionDayRequest(
        int CalendarId,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime);
}
