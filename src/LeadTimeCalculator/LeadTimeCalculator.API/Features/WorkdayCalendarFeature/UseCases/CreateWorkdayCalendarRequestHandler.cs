using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases
{
    internal sealed class CreateWorkdayCalendarRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public CreateWorkdayCalendarRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task<CreateWorkdayCalendarResponse> HandleAsync(
            CreateWorkdayCalendarRequest request,
            CancellationToken cancellationToken = default)
        {
            var calendar = await _workdayCalendarRepository.CreateAsync(
                request.DefaultWorkdayStartTime,
                request.DefaultWorkdayEndTime);

            var response = new CreateWorkdayCalendarResponse(
                calendar.Id);

            return response;
        }
    }
}
