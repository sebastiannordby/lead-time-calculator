using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddExceptionDay;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.AddExceptionDay
{
    public sealed class AddWorkdayCalendarExceptionDayRequestValidator
        : AbstractValidator<AddWorkdayCalendarExceptionDayRequest>
    {
        public AddWorkdayCalendarExceptionDayRequestValidator()
        {
            RuleFor(x => x.CalendarId)
                .GreaterThan(0);
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime);
        }
    }
}
