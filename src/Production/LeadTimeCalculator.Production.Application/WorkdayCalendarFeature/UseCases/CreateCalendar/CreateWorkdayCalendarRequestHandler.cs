using FluentValidation;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.Contracts;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.Production.Domain.Models.WorkdayCalendar;
using LeadTimeCalculator.Production.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.CreateCalendar
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
