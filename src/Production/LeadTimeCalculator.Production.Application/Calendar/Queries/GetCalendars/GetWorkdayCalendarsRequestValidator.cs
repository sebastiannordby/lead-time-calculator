using FluentValidation;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.Calendar.Queries.GetCalendars
{
    internal sealed class GetWorkdayCalendarsRequestValidator
        : AbstractValidator<GetWorkdayCalendarsRequest>
    {
        public GetWorkdayCalendarsRequestValidator()
        {

        }
    }
}
