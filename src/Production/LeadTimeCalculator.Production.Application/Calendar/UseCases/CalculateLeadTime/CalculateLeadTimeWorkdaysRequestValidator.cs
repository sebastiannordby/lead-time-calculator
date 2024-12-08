using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateLeadTime;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateLeadTime
{
    public class CalculateLeadTimeWorkdaysRequestValidator
        : AbstractValidator<CalculateLeadTimeWorkdaysRequest>
    {
        public CalculateLeadTimeWorkdaysRequestValidator()
        {
            RuleFor(x => x.CalendarId)
                .GreaterThan(0);
        }
    }
}
