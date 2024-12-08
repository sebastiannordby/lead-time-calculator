using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.AddExceptionDay
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
