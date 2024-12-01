using LeadTimeCalculator.API.Domain.Shared.Exceptions;
using System.Collections.ObjectModel;

namespace LeadTimeCalculator.API.Domain.WorkdayCalendarFeature
{
    public class WorkdayCalendar
    {
        public int Id { get; private set; }
        private readonly Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> _defaultWorkHours;
        private readonly List<Holiday> _holidays;
        private readonly List<ExceptionDay> _exceptionDays;
        private readonly double _defaultWorkhoursPerDay = 8;

        public IReadOnlyCollection<Holiday> Holidays =>
            _holidays.AsReadOnly();
        public IReadOnlyCollection<ExceptionDay> ExceptionDays =>
            _exceptionDays.AsReadOnly();
        public ReadOnlyDictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> DefaultWorkHours =>
            _defaultWorkHours.AsReadOnly();

        public WorkdayCalendar(
            int id,
            TimeSpan defaultWorkdayStartTime,
            TimeSpan defaultWorkdayEndTime)
        {
            if (id <= 0)
                throw new DomainException(
                    "Id cannot be less than or equal to 0");
            if (defaultWorkdayEndTime < defaultWorkdayStartTime)
                throw new DomainException(
                    $"{defaultWorkdayEndTime} cannot be less than {defaultWorkdayStartTime}");

            var defaultNumberOfWorkingHoursPerDay =
                (defaultWorkdayEndTime - defaultWorkdayStartTime).Hours;
            if (defaultNumberOfWorkingHoursPerDay < 1)
                throw new DomainException(
                    "Number of hours of work per day must be greater than or equal to 1");

            Id = id;

            _defaultWorkHours = new Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)>
            {
                { DayOfWeek.Monday, (defaultWorkdayStartTime, defaultWorkdayEndTime) },
                { DayOfWeek.Tuesday, (defaultWorkdayStartTime, defaultWorkdayEndTime) },
                { DayOfWeek.Wednesday, (defaultWorkdayStartTime, defaultWorkdayEndTime) },
                { DayOfWeek.Thursday, (defaultWorkdayStartTime, defaultWorkdayEndTime) },
                { DayOfWeek.Friday, (defaultWorkdayStartTime, defaultWorkdayEndTime) }
            };
            _holidays = new List<Holiday>();
            _exceptionDays = new List<ExceptionDay>();
        }

        public WorkdayCalendar(
            double defaultWorkhoursPerDay,
            Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> defaultWorkHours,
            IEnumerable<Holiday> holidays)
        {
            _defaultWorkhoursPerDay = defaultWorkhoursPerDay;
            _defaultWorkHours = defaultWorkHours;
            _holidays = holidays.ToList();
            _exceptionDays = new List<ExceptionDay>();
        }

        public void AddExceptionDay(ExceptionDay exception)
        {
            _exceptionDays.Add(exception);
        }

        public void AddHoliday(Holiday holiday)
        {
            if (holiday is null)
                throw new DomainException("Holiday must have a value");
            if (_holidays.Any(x => x.Matches(holiday.Date)))
                throw new DomainException($"{holiday.Date} is already added");

            _holidays.Add(holiday);
        }

        public DateTime CalculateWhenCanShipWhenProductionStartsAt(
            DateTime startProductionAt, double productionTimeWorkdayFractions)
        {
            if (productionTimeWorkdayFractions < 0)
                throw new DomainException($"{nameof(productionTimeWorkdayFractions)} must be greater than 0");
            if (productionTimeWorkdayFractions == 0)
                return startProductionAt;

            var remainingWorkdays = productionTimeWorkdayFractions;
            var currentWorkday = GetNextValidWorkday(startProductionAt, WorkdayTraversal.ForwardsInTime);
            var currentProductionTime = startProductionAt;

            while (remainingWorkdays > 0)
            {
                if (currentWorkday.Day == 27)
                {
                    string e = "";
                }

                if (currentWorkday.Day == 24)
                {
                    string e = "";
                }

                var currentDayWorkHours = TryGetWorkingHours(currentWorkday);
                var currentDayStart = currentDayWorkHours.Start;
                var currentDayEnd = currentDayWorkHours.End;

                // Adjust the start time for the first production day
                if (currentWorkday.Date == startProductionAt.Date)
                {
                    currentDayStart = startProductionAt.TimeOfDay > currentDayStart
                        ? startProductionAt.TimeOfDay
                        : currentDayStart;
                }

                // Calculate available working time for the day
                var availableWorkHours = (currentDayEnd - currentDayStart).TotalHours;
                var availableWorkHoursAsWorkdayFraction = availableWorkHours / _defaultWorkhoursPerDay;

                if (remainingWorkdays <= availableWorkHoursAsWorkdayFraction)
                {
                    // Production finishes on this day
                    var hoursToComplete = remainingWorkdays * _defaultWorkhoursPerDay;
                    var productionEndTime = currentDayStart + TimeSpan.FromHours(hoursToComplete);

                    return ResetToWholeSecond(currentWorkday.Date + productionEndTime);
                }

                // Move to the next workday
                remainingWorkdays -= availableWorkHoursAsWorkdayFraction;
                currentWorkday = GetNextValidWorkday(currentWorkday.AddDays(1), WorkdayTraversal.ForwardsInTime);
            }

            throw new InvalidOperationException("Production time exceeds calculated workdays");
        }


        public DateTime CalculateProductionTimeWhenHaveToShipAt(
            DateTime shippingAt, double productionTimeWorkdayFractions)
        {
            if (productionTimeWorkdayFractions < 0)
                throw new DomainException($"{nameof(productionTimeWorkdayFractions)} must be greater than 0");
            if (productionTimeWorkdayFractions == 0)
                return shippingAt;

            var remainingWorkdays = productionTimeWorkdayFractions;
            var currentWorkday = GetNextValidWorkday(shippingAt, WorkdayTraversal.BackwardsInTime);
            var currentProductionTime = shippingAt;

            while (remainingWorkdays > 0)
            {
                var currentDayWorkHours = TryGetWorkingHours(currentWorkday);
                var currentDayStart = currentDayWorkHours.Start;
                var currentDayEnd = currentDayWorkHours.End;

                // Adjust the end time for the last day (wanted delivery day)
                if (currentWorkday.Date == shippingAt.Date)
                {
                    currentDayEnd = shippingAt.TimeOfDay < currentDayEnd
                        ? shippingAt.TimeOfDay
                        : currentDayEnd;
                }

                // Calculate available working time for the day
                var availableWorkHours = (currentDayEnd - currentDayStart).TotalHours;
                var availableWorkHoursAsWorkdayFraction = availableWorkHours / _defaultWorkhoursPerDay;

                if (remainingWorkdays <= availableWorkHoursAsWorkdayFraction)
                {
                    // Production starts on this day
                    var hoursToStart = remainingWorkdays * _defaultWorkhoursPerDay; // Use actual available hours
                    var productionStartTime = currentDayEnd - TimeSpan.FromHours(hoursToStart);
                    return ResetToWholeSecond(currentWorkday.Date + productionStartTime);
                }

                // Move to the previous workday
                remainingWorkdays -= availableWorkHoursAsWorkdayFraction;
                currentWorkday = GetNextValidWorkday(currentWorkday.AddDays(-1), WorkdayTraversal.BackwardsInTime);
            }

            throw new InvalidOperationException("Production time exceeds calculated workdays");
        }

        private DateTime ResetToWholeSecond(DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMinute));
        }

        private DateTime GetNextValidWorkday(DateTime date, WorkdayTraversal traversalType)
        {
            var movingDate = date;

            while (true)
            {
                // Check if the day has valid working hours
                var workingHours = TryGetWorkingHours(movingDate);

                if (workingHours != default)
                {
                    var currentTime = movingDate.TimeOfDay;

                    // If moving forward in time, check if we are after working hours
                    if (traversalType == WorkdayTraversal.ForwardsInTime && currentTime > workingHours.End)
                    {
                        // Move to the next valid working day
                        movingDate = movingDate.AddDays(1).Date.Add(workingHours.Start);
                        continue; // Recheck the new day
                    }
                    else if (traversalType == WorkdayTraversal.BackwardsInTime && currentTime < workingHours.Start) // If moving backward in time, check if we are before working hours
                    {
                        // Move to the previous valid working day
                        movingDate = movingDate.AddDays(-1).Date.Add(workingHours.Start);
                        continue; // Recheck the new day
                    }

                    // If the current time is within working hours, return the date
                    return movingDate;
                }

                // If no valid working hours, move to the next/previous day
                movingDate = traversalType == WorkdayTraversal.ForwardsInTime
                    ? movingDate.AddDays(1)
                    : movingDate.AddDays(-1);
            }
        }

        private (TimeSpan Start, TimeSpan End) TryGetWorkingHours(DateTime date)
        {
            var exceptionDay = _exceptionDays
                .FirstOrDefault(e => e.Date == date.Date);
            if (exceptionDay != null)
            {
                return (exceptionDay.StartTime, exceptionDay.EndTime);
            }

            if (_holidays.Any(h => h.Matches(date)))
            {
                return default;
            }

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return default;
            }

            if (_defaultWorkHours.TryGetValue(date.DayOfWeek, out var hours))
            {
                return hours;
            }

            return default;
        }

        private enum WorkdayTraversal
        {
            ForwardsInTime = 0,
            BackwardsInTime = 1
        }
    }
}
