using FluentValidation;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Domain.Repositories.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.AddExceptionDay
{
    public sealed class AddWorkdayCalendarExceptionDayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<AddWorkdayCalendarExceptionDayRequest> _requestValidator;

        public AddWorkdayCalendarExceptionDayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<AddWorkdayCalendarExceptionDayRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task HandleAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new ArgumentException($"No WorkdayCalendar with given Id({request.CalendarId})");

            calendar.AddExceptionDay(new ExceptionDay(
                request.Date,
                request.StartTime,
                request.EndTime));

            await _workdayCalendarRepository
                .SaveAsync(calendar, cancellationToken);
        }
    }
}
