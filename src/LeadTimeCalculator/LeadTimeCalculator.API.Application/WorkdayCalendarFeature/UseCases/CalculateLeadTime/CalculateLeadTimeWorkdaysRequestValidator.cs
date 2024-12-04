using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases.CalculateLeadTime
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
