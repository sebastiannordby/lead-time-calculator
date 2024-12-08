using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CreateCalendar
{
    public sealed class CreateWorkdayCalendarRequestValidator
        : AbstractValidator<CreateWorkdayCalendarRequest>
    {
        public CreateWorkdayCalendarRequestValidator()
        {
            RuleFor(x => x.DefaultWorkdayEndTime)
                .GreaterThan(x => x.DefaultWorkdayStartTime);
        }
    }
}
