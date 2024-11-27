using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases
{
    public sealed class AddWorkdayCalendarExceptionDayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public AddWorkdayCalendarExceptionDayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task HandleAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken)
        {
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
