using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateTimeBackward
{
    public class CalculateWorkdayCalendarTimeBackwardRequestValidator
        : AbstractValidator<CalculateWorkdayCalendarTimeBackwardRequest>
    {
        public CalculateWorkdayCalendarTimeBackwardRequestValidator()
        {
            RuleFor(x => x.WorkdayCalendarId)
                .GreaterThan(0);
            RuleFor(x => x.DateTimeToSubtractFrom)
                .NotEmpty();
            RuleFor(x => x.WorkdaysToSubtract)
                .GreaterThan(0);
        }
    }
}
