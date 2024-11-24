namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models
{
    public class WorkdayCalendar
    {
        private readonly Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> _defaultWorkHours;
        private readonly List<Holiday> _holidays;
        private readonly List<ExceptionDay> _exceptionDays;
        private readonly Dictionary<DateTime, WorkingDay> _specificDays;
        private readonly double DefaulWorkhoursPerDay = 8;

        public WorkdayCalendar(
            Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> defaultWorkHours,
            IEnumerable<Holiday> holidays)
        {
            _defaultWorkHours = defaultWorkHours;
            _holidays = holidays.ToList();
            _exceptionDays = new List<ExceptionDay>();
            _specificDays = new Dictionary<DateTime, WorkingDay>();
        }

        public void AddExceptionDay(ExceptionDay exception)
        {
            _exceptionDays.Add(exception);
        }

        public void SetSpecificWorkingDay(WorkingDay workingDay)
        {
            _specificDays[workingDay.Date] = workingDay;
        }

        public DateTime AddFractionalWorkingDays(DateTime start, double fractionalDays)
        {
            if (fractionalDays == 0)
                return start;

            var isAdding = fractionalDays > 0;
            var remainingDays = Math.Abs(fractionalDays);
            var currentWorkday = GetNextValidWorkday(start, isAdding);

            while (remainingDays > 0)
            {
                TryGetWorkingHours(currentWorkday, out var workHours);

                var currentWorkdayStartTime = currentWorkday.Date + workHours.Start;
                var currentWorkdayEndTime = currentWorkday.Date + workHours.End;

                if (currentWorkday < currentWorkdayStartTime)
                {
                    currentWorkday = currentWorkdayStartTime;
                }
                else if (currentWorkday > currentWorkdayEndTime)
                {
                    currentWorkday = currentWorkdayEndTime;
                }

                var availableTime = isAdding
                    ? currentWorkdayEndTime - currentWorkday
                    : currentWorkdayEndTime - currentWorkdayStartTime;
                if (availableTime < TimeSpan.Zero)
                {
                    currentWorkday = GetNextValidWorkday(
                        isAdding ? currentWorkday.Date.AddDays(1) : currentWorkday.Date.AddDays(-1),
                        isAdding);
                    continue;
                }

                var workDayDuration = workHours.End - workHours.Start;
                if (availableTime > workDayDuration)
                {
                    availableTime = workDayDuration;
                }

                var availableFractionOfWorkday = availableTime.TotalHours / DefaulWorkhoursPerDay;

                if (remainingDays <= availableFractionOfWorkday)
                {
                    var hoursToAdjust = remainingDays * DefaulWorkhoursPerDay;

                    return isAdding
                        ? ResetToWholeSecond(currentWorkday.AddHours(hoursToAdjust))
                        : ResetToWholeSecond(currentWorkdayEndTime.AddHours(-hoursToAdjust));
                }


                remainingDays -= availableFractionOfWorkday;
                currentWorkday = GetNextValidWorkday(
                    isAdding ? currentWorkday.Date.AddDays(1) : currentWorkday.Date.AddDays(-1),
                    isAdding);
            }

            return currentWorkday;
        }

        private DateTime ResetToWholeSecond(DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMinute));
        }

        private DateTime GetNextValidWorkday(DateTime date, bool isAdding)
        {
            while (!TryGetWorkingHours(date, out _))
            {
                date = isAdding ? date.AddDays(1) : date.AddDays(-1);
            }

            return date;
        }

        private bool TryGetWorkingHours(DateTime date, out (TimeSpan Start, TimeSpan End) workHours)
        {
            var exceptionDay = _exceptionDays.FirstOrDefault(e => e.Date == date.Date);
            if (exceptionDay != null)
            {
                workHours = (exceptionDay.StartTime, exceptionDay.EndTime);
                return true;
            }

            if (_holidays.Any(h => h.Matches(date)))
            {
                workHours = default;
                return false;
            }

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                workHours = default;
                return false;
            }

            if (_defaultWorkHours.TryGetValue(date.DayOfWeek, out var hours))
            {
                workHours = hours;
                return true;
            }

            workHours = default;
            return false;
        }
    }
}
