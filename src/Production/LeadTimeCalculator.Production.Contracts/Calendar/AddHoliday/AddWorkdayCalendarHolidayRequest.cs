namespace LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday
{
    public sealed record AddWorkdayCalendarHolidayRequest(
        int CalendarId,
        DateTime Date,
        bool IsRecurring);
}
