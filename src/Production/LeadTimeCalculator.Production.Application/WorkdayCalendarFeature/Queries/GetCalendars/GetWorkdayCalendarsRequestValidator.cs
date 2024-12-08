using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.GetCalendars
{
    internal sealed class GetWorkdayCalendarsRequestValidator
        : AbstractValidator<GetWorkdayCalendarsRequest>
    {
        public GetWorkdayCalendarsRequestValidator()
        {

        }
    }
}
