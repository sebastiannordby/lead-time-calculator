using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.AddHoliday
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
