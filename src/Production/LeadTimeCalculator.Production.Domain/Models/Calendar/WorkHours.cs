using LeadTimeCalculator.Production.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.Production.Domain.Models.Calendar
{
    public sealed class WorkHours
    {
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public WorkHours(
            TimeSpan startTime,
            TimeSpan endTime)
        {
            if (startTime == endTime)
                throw new DomainException($"{nameof(startTime)} cannot be equal to {nameof(endTime)}");
            if (endTime < startTime)
                throw new DomainException($"{nameof(endTime)} cannot be less than {nameof(startTime)}");

            StartTime = startTime;
            EndTime = endTime;
        }

        public double GetWorkingHours()
        {
            return (StartTime - EndTime).TotalHours;
        }
    }
}
