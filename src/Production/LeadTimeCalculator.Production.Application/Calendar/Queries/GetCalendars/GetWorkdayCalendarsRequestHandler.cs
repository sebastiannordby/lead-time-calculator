using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.Queries.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.Calendar.Queries.GetCalendars
{
    public sealed class GetWorkdayCalendarsRequestHandler
    {
        private readonly IQueryWorkdayCalendarDetailedView _queryCalendarDetailedView;
        private readonly IValidator<GetWorkdayCalendarsRequest> _requestValidator;

        public GetWorkdayCalendarsRequestHandler(
            IQueryWorkdayCalendarDetailedView queryCalendarDetailedView,
            IValidator<GetWorkdayCalendarsRequest> requestValidator)
        {
            _queryCalendarDetailedView = queryCalendarDetailedView;
            _requestValidator = requestValidator;
        }

        public async Task<GetWorkdayCalendarsResponse> HandleAsync(
            GetWorkdayCalendarsRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendarDetailViews = await _queryCalendarDetailedView
                .GetCalendarsAsync(cancellationToken);

            var response = new GetWorkdayCalendarsResponse(
                calendarDetailViews);

            return response;
        }
    }
}
