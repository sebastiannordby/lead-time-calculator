using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases.AddExceptionDay
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
