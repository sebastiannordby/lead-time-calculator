using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;
using LeadTimeCalculator.API.Shared.Exceptions;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases
{
    internal sealed class CalculateLeadTimeWorkdaysRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public CalculateLeadTimeWorkdaysRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        internal async Task<CalculateLeadTimeWorkdaysResponse> HandleAsync(
            CalculateLeadTimeWorkdaysRequest request,
            CancellationToken cancellationToken = default)
        {
            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new DomainException($"No WorkdayCalendar with given Id({request.CalendarId})");

            var leadTime = calendar
                .CalculateLeadTimeWorkdays(request.StartingDate, request.WorkdaysAdjustment);

            var response = new CalculateLeadTimeWorkdaysResponse(leadTime);

            return response;
        }
    }
}
