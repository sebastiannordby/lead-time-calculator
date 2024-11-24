namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday
{
    public sealed record AddWorkdayCalendarHolidayRequest(
        int CalendarId,
        DateTime Date,
        bool IsRecurring);
}
