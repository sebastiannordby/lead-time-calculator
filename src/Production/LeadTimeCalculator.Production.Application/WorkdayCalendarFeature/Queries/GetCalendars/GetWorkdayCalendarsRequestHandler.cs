using FluentValidation;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.Contracts;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.GetCalendars
{
    public sealed class GetWorkdayCalendarsRequestHandler
    {
        private readonly IQueryCalendarDetailedView _queryCalendarDetailedView;
        private readonly IValidator<GetWorkdayCalendarsRequest> _requestValidator;

        public GetWorkdayCalendarsRequestHandler(
            IQueryCalendarDetailedView queryCalendarDetailedView,
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
