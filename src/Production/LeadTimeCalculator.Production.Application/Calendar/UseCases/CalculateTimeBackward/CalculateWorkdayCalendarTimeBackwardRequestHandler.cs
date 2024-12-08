using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateTimeBackward
{
    public sealed class CalculateWorkdayCalendarTimeBackwardRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<CalculateWorkdayCalendarTimeBackwardRequest> _requestValidator;

        public CalculateWorkdayCalendarTimeBackwardRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<CalculateWorkdayCalendarTimeBackwardRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task<CalculateWorkdayCalendarTimeBackwardResponse> HandleAsync(
            CalculateWorkdayCalendarTimeBackwardRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendar = await _workdayCalendarRepository
                .FindAsync(request.WorkdayCalendarId, cancellationToken);
            if (calendar is null)
                throw new ArgumentException($"No WorkdayCalendar with given Id({request.WorkdayCalendarId})");

            var subtractedDateTime = calendar
                .SubtractWorkingDays(request.DateTimeToSubtractFrom, request.WorkdaysToSubtract);

            var response = new CalculateWorkdayCalendarTimeBackwardResponse(
                StartDateTime: request.DateTimeToSubtractFrom,
                EndDateTime: subtractedDateTime,
                WorkdaysSubtracted: request.WorkdaysToSubtract);

            return response;
        }
    }
}
