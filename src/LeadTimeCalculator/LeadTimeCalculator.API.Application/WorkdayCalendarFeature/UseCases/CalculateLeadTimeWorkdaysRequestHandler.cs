using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases
{
    public sealed class CalculateLeadTimeWorkdaysRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public CalculateLeadTimeWorkdaysRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task<CalculateLeadTimeWorkdaysResponse> HandleAsync(
            CalculateLeadTimeWorkdaysRequest request,
            CancellationToken cancellationToken = default)
        {
            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new ArgumentException($"No WorkdayCalendar with given Id({request.CalendarId})");

            var leadTime = calendar
                .CalculateLeadTimeWorkdays(request.StartingDate, request.WorkdaysAdjustment);

            var response = new CalculateLeadTimeWorkdaysResponse(leadTime);

            return response;
        }
    }
}
