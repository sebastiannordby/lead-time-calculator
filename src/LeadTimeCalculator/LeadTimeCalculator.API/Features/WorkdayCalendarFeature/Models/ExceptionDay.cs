using LeadTimeCalculator.API.Shared.Exceptions;

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
                throw new DomainException("End time must be after start time.");
            if (date == DateTime.MinValue)
                throw new DomainException("Date must have value.");
            if (date == DateTime.MaxValue)
                throw new DomainException("Invalid date.");

            Date = date.Date;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}