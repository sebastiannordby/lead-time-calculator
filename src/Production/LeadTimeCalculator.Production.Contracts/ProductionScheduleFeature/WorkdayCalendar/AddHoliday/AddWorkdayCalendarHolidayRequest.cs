namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday
{
    public sealed record AddWorkdayCalendarHolidayRequest(
        int CalendarId,
        DateTime Date,
        bool IsRecurring);
}
