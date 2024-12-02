using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.WorkdayCalendarFeature
{
    public sealed class WorkWeek
    {
        public required WorkHours MondayWorkingHours { get; init; }
        public required WorkHours TuesdayWorkingHours { get; init; }
        public required WorkHours WednesdayWorkingHours { get; init; }
        public required WorkHours ThursdayWorkingHours { get; init; }
        public required WorkHours FridayWorkingHours { get; init; }

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
