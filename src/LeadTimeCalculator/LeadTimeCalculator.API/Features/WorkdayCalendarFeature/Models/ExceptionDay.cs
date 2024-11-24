namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models
{
    public class ExceptionDay
    {
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public ExceptionDay(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            Date = date.Date;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}