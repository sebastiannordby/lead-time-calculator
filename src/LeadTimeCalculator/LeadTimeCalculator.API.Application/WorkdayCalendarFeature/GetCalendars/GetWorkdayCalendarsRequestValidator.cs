using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.GetCalendars
{
    internal sealed class GetWorkdayCalendarsRequestValidator
        : AbstractValidator<GetWorkdayCalendarsRequest>
    {
        public GetWorkdayCalendarsRequestValidator()
        {

        }
    }
}
