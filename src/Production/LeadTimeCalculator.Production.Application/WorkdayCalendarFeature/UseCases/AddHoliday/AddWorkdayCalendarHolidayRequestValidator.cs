using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.AddHoliday
{
    public class AddWorkdayCalendarHolidayRequestValidator
        : AbstractValidator<AddWorkdayCalendarHolidayRequest>
    {
        public AddWorkdayCalendarHolidayRequestValidator()
        {
            RuleFor(x => x.CalendarId)
                .GreaterThan(0);
        }
    }
}
