using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;
using LeadTimeCalculator.API.Shared.Exceptions;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases
{
    internal sealed class AddWorkdayCalendarExceptionDayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public AddWorkdayCalendarExceptionDayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        internal async Task HandleAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken)
        {
            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new DomainException($"No WorkdayCalendar with given Id({request.CalendarId})");

            calendar.AddExceptionDay(new ExceptionDay(
                request.Date,
                request.StartTime,
                request.EndTime));

            await _workdayCalendarRepository
                .SaveAsync(calendar, cancellationToken);
        }
    }
}
