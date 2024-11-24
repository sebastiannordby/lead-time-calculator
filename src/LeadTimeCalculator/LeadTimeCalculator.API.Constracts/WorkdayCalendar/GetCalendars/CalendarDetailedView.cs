namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars
{
    public sealed class CalendarDetailedView
    {
        public required int Id { get; init; }
        public required HolidayView[] Holidays { get; init; }
        public required ExceptionDayView[] ExceptionDays { get; init; }
        public required DefaultWorkingDayView[] DefaultWorkingDays { get; init; }

        public class DefaultWorkingDayView
        {
            public required DayOfWeek DayOfWeek { get; set; }
            public required TimeSpan StartTime { get; init; }
            public required TimeSpan EndTime { get; init; }
            public TimeSpan WorkDuration => EndTime - StartTime;
        }

        public class ExceptionDayView
        {
            public required DateTime Date { get; init; }
            public required TimeSpan StartTime { get; init; }
            public required TimeSpan EndTime { get; init; }
        }

        public class HolidayView
        {
            public DateTime Date { get; init; }
            public bool IsRecurring { get; init; }
        }
    }
}
