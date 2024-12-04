using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Domain.Repositories.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.CreateCalendar
{
    public sealed class CreateWorkdayCalendarRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<CreateWorkdayCalendarRequest> _requestValidator;

        public CreateWorkdayCalendarRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<CreateWorkdayCalendarRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task<CreateWorkdayCalendarResponse> HandleAsync(
            CreateWorkdayCalendarRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            try
            {
                var calendarId = await _workdayCalendarRepository
                    .GetNewCalendarNumberAsync(cancellationToken);

                var calendar = new WorkdayCalendar(
                    calendarId,
                    request.DefaultWorkdayStartTime,
                    request.DefaultWorkdayEndTime);

                var response = new CreateWorkdayCalendarResponse(
                    calendar.Id);

                await _workdayCalendarRepository
                    .SaveAsync(calendar, cancellationToken);

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
