namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models
{
    public class WorkingDay
    {
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public TimeSpan WorkDuration => EndTime - StartTime;

        public WorkingDay(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            Date = date.Date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public void UpdateWorkingHours(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} - {StartTime:hh\\:mm} to {EndTime:hh\\:mm} ({WorkDuration.TotalHours} hours)";
        }
    }
}