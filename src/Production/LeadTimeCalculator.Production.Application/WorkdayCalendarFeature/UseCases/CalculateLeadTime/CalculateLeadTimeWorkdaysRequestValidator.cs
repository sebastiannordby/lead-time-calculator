using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CalculateLeadTime;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.CalculateLeadTime
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
