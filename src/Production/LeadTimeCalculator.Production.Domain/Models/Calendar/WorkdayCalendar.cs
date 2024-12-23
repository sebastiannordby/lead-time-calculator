﻿
using LeadTimeCalculator.Production.Domain.Exceptions;

namespace LeadTimeCalculator.Production.Domain.Models.Calendar
{
    public sealed class WorkdayCalendar
    {
        public int Id { get; private set; }
        private readonly WorkWeek _workWeek;
        private readonly List<Holiday> _holidays;
        private readonly List<ExceptionDay> _exceptionDays;
        private readonly double _defaultWorkhoursPerDay = 8;

        public IReadOnlyCollection<Holiday> Holidays =>
            _holidays.AsReadOnly();
        public IReadOnlyCollection<ExceptionDay> ExceptionDays =>
            _exceptionDays.AsReadOnly();
        public WorkWeek WorkWeek => _workWeek;

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

            _workWeek = new WorkWeek(
                new(defaultWorkdayStartTime, defaultWorkdayEndTime));
            _holidays = new List<Holiday>();
            _exceptionDays = new List<ExceptionDay>();
        }

        public WorkdayCalendar(
            double defaultWorkhoursPerDay,
            WorkWeek defaultWorkHours,
            IEnumerable<Holiday> holidays)
        {
            _defaultWorkhoursPerDay = defaultWorkhoursPerDay;
            _workWeek = defaultWorkHours;
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

        public DateTime AddWorkingDays(
            DateTime startDate, double fractionalWorkdays)
        {
            if (fractionalWorkdays < 0)
                throw new DomainException($"{nameof(fractionalWorkdays)} must be greater than 0");
            if (fractionalWorkdays == 0)
                return startDate;

            var remainingWorkdays = fractionalWorkdays;
            var currentWorkday = GetNextValidWorkday(startDate);
            var currentProductionTime = startDate;

            while (remainingWorkdays > 0)
            {
                var currentDayWorkHours = TryGetWorkingHours(currentWorkday)!;
                var currentDayStart = currentDayWorkHours.StartTime;
                var currentDayEnd = currentDayWorkHours.EndTime;

                // Adjust the start time for the first production day
                if (currentWorkday.Date == startDate.Date)
                {
                    currentDayStart = startDate.TimeOfDay > currentDayStart
                        ? startDate.TimeOfDay
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
                currentWorkday = GetNextValidWorkday(currentWorkday.AddDays(1));
            }

            throw new InvalidOperationException("Production time exceeds calculated workdays");
        }


        public DateTime SubtractWorkingDays(
            DateTime startDate, double fractionalWorkdays)
        {
            if (fractionalWorkdays < 0)
                throw new DomainException($"{nameof(fractionalWorkdays)} must be greater than 0");
            if (fractionalWorkdays == 0)
                return startDate;

            var remainingWorkdays = fractionalWorkdays;
            var currentWorkday = GetPreviousValidWorkday(startDate);
            var currentProductionTime = startDate;

            while (remainingWorkdays > 0)
            {
                var currentDayWorkHours = TryGetWorkingHours(currentWorkday)!;
                var currentDayStart = currentDayWorkHours.StartTime;
                var currentDayEnd = currentDayWorkHours.EndTime;

                // Adjust the end time for the last day wanted shipping day
                if (currentWorkday.Date == startDate.Date)
                {
                    currentDayEnd = startDate.TimeOfDay < currentDayEnd
                        ? startDate.TimeOfDay
                        : currentDayEnd;
                }

                // Calculate available working time for the day
                var availableWorkHours = (currentDayEnd - currentDayStart).TotalHours;
                var availableWorkHoursAsWorkdayFraction = availableWorkHours / _defaultWorkhoursPerDay;

                if (remainingWorkdays <= availableWorkHoursAsWorkdayFraction)
                {
                    // Production starts on this day
                    var hoursToStart = remainingWorkdays * _defaultWorkhoursPerDay;
                    var productionStartTime = currentDayEnd - TimeSpan.FromHours(hoursToStart);
                    return ResetToWholeSecond(currentWorkday.Date + productionStartTime);
                }

                // Move to the previous workday
                remainingWorkdays -= availableWorkHoursAsWorkdayFraction;
                currentWorkday = GetPreviousValidWorkday(currentWorkday.AddDays(-1));
            }

            throw new InvalidOperationException("Production time exceeds calculated workdays");
        }

        private DateTime ResetToWholeSecond(DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMinute));
        }

        private DateTime GetNextValidWorkday(DateTime date)
        {
            var movingDate = date;

            while (true)
            {
                // Check if the day has valid working hours
                var workingHours = TryGetWorkingHours(movingDate);

                if (workingHours != default)
                {
                    var currentTime = movingDate.TimeOfDay;

                    // Check if we are after working hours
                    if (currentTime > workingHours.EndTime)
                    {
                        // Move to the next valid working day
                        movingDate = movingDate.AddDays(1).Date.Add(workingHours.StartTime);
                        continue;
                    }

                    return movingDate;
                }

                // If no valid working hours, move to the next day
                movingDate = movingDate.AddDays(1);
            }
        }

        private DateTime GetPreviousValidWorkday(DateTime date)
        {
            var movingDate = date;

            while (true)
            {
                // Check if the day has valid working hours
                var workingHours = TryGetWorkingHours(movingDate);

                if (workingHours != default)
                {
                    var currentTime = movingDate.TimeOfDay;

                    // Check if we are before working hours
                    if (currentTime < workingHours.StartTime)
                    {
                        // Move to the previous valid working day
                        movingDate = movingDate.AddDays(-1).Date.Add(workingHours.StartTime);
                        continue;
                    }

                    return movingDate;
                }

                // If no valid working hours, move to the previous day
                movingDate = movingDate.AddDays(-1);
            }
        }

        private WorkHours? TryGetWorkingHours(DateTime date)
        {
            var exceptionDay = _exceptionDays
                .FirstOrDefault(e => e.Date == date.Date);
            if (exceptionDay != null)
            {
                return new(exceptionDay.StartTime, exceptionDay.EndTime);
            }

            if (_holidays.Any(h => h.Matches(date)))
            {
                return default;
            }

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return default;
            }

            var workingHours = _workWeek
                .GetByDayOfWeek(date.DayOfWeek);

            return workingHours;
        }
    }
}
