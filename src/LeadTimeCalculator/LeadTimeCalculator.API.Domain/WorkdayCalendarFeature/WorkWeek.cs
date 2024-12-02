using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.WorkdayCalendarFeature
{
    /// <summary>
    /// Represents a week of working Monday-Friday.
    /// </summary>
    public sealed class WorkWeek
    {
        public WorkHours? MondayWorkingHours { get; }
        public WorkHours? TuesdayWorkingHours { get; }
        public WorkHours? WednesdayWorkingHours { get; }
        public WorkHours? ThursdayWorkingHours { get; }
        public WorkHours? FridayWorkingHours { get; }

        /// <summary>
        /// Creates a work week with defined work hours
        /// defined for each respective work day.
        /// </summary>
        /// <param name="mondayWorkingHours"></param>
        /// <param name="tuesdayWorkingHours"></param>
        /// <param name="wednesdayWorkingHours"></param>
        /// <param name="thursdayWorkingHours"></param>
        /// <param name="fridayWorkingHours"></param>
        public WorkWeek(
            WorkHours? mondayWorkingHours,
            WorkHours? tuesdayWorkingHours,
            WorkHours? wednesdayWorkingHours,
            WorkHours? thursdayWorkingHours,
            WorkHours? fridayWorkingHours)
        {
            MondayWorkingHours = mondayWorkingHours;
            TuesdayWorkingHours = tuesdayWorkingHours;
            WednesdayWorkingHours = wednesdayWorkingHours;
            ThursdayWorkingHours = thursdayWorkingHours;
            FridayWorkingHours = fridayWorkingHours;
        }

        /// <summary>
        /// Creates a work week with the same work hours
        /// for all the working days.
        /// </summary>
        /// <param name="sameWorkhoursForWholeWeek"></param>
        public WorkWeek(
            WorkHours sameWorkhoursForWholeWeek)
        {
            if (sameWorkhoursForWholeWeek is null)
                throw new DomainException($"{nameof(sameWorkhoursForWholeWeek)} must have a value.");

            MondayWorkingHours = sameWorkhoursForWholeWeek;
            TuesdayWorkingHours = sameWorkhoursForWholeWeek;
            WednesdayWorkingHours = sameWorkhoursForWholeWeek;
            ThursdayWorkingHours = sameWorkhoursForWholeWeek;
            FridayWorkingHours = sameWorkhoursForWholeWeek;
        }

        /// <summary>
        /// Returns the working hours for a given workday.
        /// Valid days: Monday-Friday.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <exception cref="DomainException"></exception>
        public WorkHours GetByDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return MondayWorkingHours;
                case DayOfWeek.Tuesday:
                    return TuesdayWorkingHours;
                case DayOfWeek.Wednesday:
                    return WednesdayWorkingHours;
                case DayOfWeek.Thursday:
                    return ThursdayWorkingHours;
                case DayOfWeek.Friday:
                    return FridayWorkingHours;

                default:
                    throw new DomainException("Only Monday-Friday are valid working days.");
            }
        }

        /// <summary>
        /// Returns working hours for Monday-Friday.
        /// </summary>
        /// <returns></returns>
        public Dictionary<DayOfWeek, WorkHours> GetWorkingWeek()
        {
            return new Dictionary<DayOfWeek, WorkHours>()
            {
                { DayOfWeek.Monday, MondayWorkingHours },
                { DayOfWeek.Tuesday, TuesdayWorkingHours},
                { DayOfWeek.Wednesday, WednesdayWorkingHours },
                { DayOfWeek.Thursday, ThursdayWorkingHours},
                { DayOfWeek.Friday, FridayWorkingHours },
            };
        }
    }
}
