using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.CreateCalendar
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
