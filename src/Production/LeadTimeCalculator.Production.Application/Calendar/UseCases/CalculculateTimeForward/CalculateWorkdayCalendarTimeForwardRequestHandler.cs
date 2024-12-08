using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculculateTimeForward
{
    public sealed class CalculateWorkdayCalendarTimeForwardRequestHandler
    {
        private readonly IValidator<CalculateWorkdayCalendarTimeForwardRequest> _requestValidator;
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public CalculateWorkdayCalendarTimeForwardRequestHandler(
            IValidator<CalculateWorkdayCalendarTimeForwardRequest> requestValidator,
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _requestValidator = requestValidator;
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task<CalculateWorkdayCalendarTimeForwardResponse> HandleAsync(
            CalculateWorkdayCalendarTimeForwardRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendar = await _workdayCalendarRepository
                .FindAsync(request.WorkdayCalendarId, cancellationToken);
            if (calendar is null)
                throw new ValidationException($"No calendar with given id({request.WorkdayCalendarId})");

            var endTime = calendar.AddWorkingDays(
                request.StartDateTime, request.WorkdaysToAdd);

            var response = new CalculateWorkdayCalendarTimeForwardResponse(
                StartDateTime: request.StartDateTime,
                EndDateTime: endTime,
                WorkdaysAdded: request.WorkdaysToAdd);

            return response;
        }
    }
}
