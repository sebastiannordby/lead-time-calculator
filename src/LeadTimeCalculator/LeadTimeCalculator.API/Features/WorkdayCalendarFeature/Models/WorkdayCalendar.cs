﻿using LeadTimeCalculator.API.Shared.Exceptions;
using System.Collections.ObjectModel;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models
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

        internal WorkdayCalendar(
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

        public DateTime CalculateLeadTimeWorkdays(
            DateTime startingDate, double workdaysAdjustment)
        {
            if (workdaysAdjustment == 0)
                return startingDate;

            var isAdding = workdaysAdjustment > 0;
            var remainingDays = Math.Abs(workdaysAdjustment);
            var currentWorkday = GetNextValidWorkday(startingDate, isAdding);

            while (remainingDays > 0)
            {
                TryGetWorkingHours(currentWorkday, out var workHours);

                var currentWorkdayStartTime = currentWorkday.Date + workHours.Start;
                var currentWorkdayEndTime = currentWorkday.Date + workHours.End;
                TimeSpan availableTime;

                if (!isAdding)
                {
                    if (currentWorkday < currentWorkdayStartTime && currentWorkday == startingDate)
                    {
                        currentWorkday = GetNextValidWorkday(currentWorkday.Date.AddDays(-1), isAdding);
                        continue;
                    }

                    if (currentWorkday > currentWorkdayStartTime && !isAdding)
                    {
                        availableTime = currentWorkday - currentWorkdayStartTime;
                    }
                    else
                    {
                        availableTime = currentWorkdayEndTime - currentWorkdayStartTime;
                    }
                }
                else
                {
                    if (currentWorkday < currentWorkdayStartTime)
                    {
                        currentWorkday = currentWorkdayStartTime;
                    }
                    else if (currentWorkday > currentWorkdayEndTime)
                    {
                        currentWorkday = currentWorkdayEndTime;
                    }

                    availableTime = currentWorkdayEndTime - currentWorkday;
                }


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

                var availableFractionOfWorkday = availableTime.TotalHours / _defaultWorkhoursPerDay;

                if (remainingDays <= availableFractionOfWorkday)
                {
                    var hoursToAdjust = remainingDays * _defaultWorkhoursPerDay;

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
            var exceptionDay = _exceptionDays
                .FirstOrDefault(e => e.Date == date.Date);
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
