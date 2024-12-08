using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.AddHoliday
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
