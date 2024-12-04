using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases.GetCalendars
{
    internal sealed class GetWorkdayCalendarsRequestValidator
        : AbstractValidator<GetWorkdayCalendarsRequest>
    {
        public GetWorkdayCalendarsRequestValidator()
        {

        }
    }
}
