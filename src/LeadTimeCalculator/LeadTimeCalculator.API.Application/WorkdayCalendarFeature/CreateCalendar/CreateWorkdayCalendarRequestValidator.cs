using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.CreateCalendar
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
