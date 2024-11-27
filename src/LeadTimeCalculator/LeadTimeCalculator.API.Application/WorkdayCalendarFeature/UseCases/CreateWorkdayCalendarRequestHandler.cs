using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases
{
    public sealed class CreateWorkdayCalendarRequestHandler
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
            try
            {
                var calendar = await _workdayCalendarRepository.CreateAsync(
                request.DefaultWorkdayStartTime,
                request.DefaultWorkdayEndTime);

                var response = new CreateWorkdayCalendarResponse(
                    calendar.Id);

                return response;
            }
            catch (DomainException ex)
            {
                // Should have used a mediator pipeline or something similar 
                // to handle the translation between exceptions,
                // or allow the API to reference domain directly.
                // Choose your tradeoffs
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
