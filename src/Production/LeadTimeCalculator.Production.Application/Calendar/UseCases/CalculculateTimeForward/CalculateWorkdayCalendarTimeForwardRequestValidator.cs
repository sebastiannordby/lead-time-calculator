using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculculateTimeForward
{
    internal sealed class CalculateWorkdayCalendarTimeForwardRequestValidator
        : AbstractValidator<CalculateWorkdayCalendarTimeForwardRequest>
    {
        public CalculateWorkdayCalendarTimeForwardRequestValidator()
        {
            RuleFor(x => x.WorkdayCalendarId)
                .GreaterThan(0);
            RuleFor(x => x.StartDateTime)
                .NotEmpty();
            RuleFor(x => x.WorkdaysToAdd)
                .GreaterThan(0);
        }
    }
}
