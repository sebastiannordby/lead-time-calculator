namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay
{
    public sealed record AddWorkdayCalendarExceptionDayRequest(
        int CalendarId,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime);
}
