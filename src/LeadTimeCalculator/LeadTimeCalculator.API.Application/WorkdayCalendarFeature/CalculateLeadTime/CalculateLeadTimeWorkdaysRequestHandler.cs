using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.CalculateLeadTime
{
    public sealed class CalculateLeadTimeWorkdaysRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<CalculateLeadTimeWorkdaysRequest> _requestValidator;

        public CalculateLeadTimeWorkdaysRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<CalculateLeadTimeWorkdaysRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task<CalculateLeadTimeWorkdaysResponse> HandleAsync(
            CalculateLeadTimeWorkdaysRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new ArgumentException($"No WorkdayCalendar with given Id({request.CalendarId})");

            var leadTime = request.WorkdaysAdjustment > 0
                ? calendar.CalculateWhenCanShipWhenProductionStartsAt(request.StartingDate, request.WorkdaysAdjustment)
                : calendar.CalculateProductionTimeWhenHaveToShipAt(request.StartingDate, -request.WorkdaysAdjustment);

            var response = new CalculateLeadTimeWorkdaysResponse(DateTime.Now);

            return response;
        }
    }
}
