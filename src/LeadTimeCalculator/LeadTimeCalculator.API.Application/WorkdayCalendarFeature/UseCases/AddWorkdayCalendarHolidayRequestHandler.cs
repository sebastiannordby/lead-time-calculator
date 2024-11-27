using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases
{
    public class AddWorkdayCalendarHolidayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;

        public AddWorkdayCalendarHolidayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
        }

        public async Task HandleAsync(
            AddWorkdayCalendarHolidayRequest request,
            CancellationToken cancellationToken = default)
        {
            var calendar = await _workdayCalendarRepository
                .FindAsync(request.CalendarId, cancellationToken);
            if (calendar is null)
                throw new ArgumentException($"No WorkdayCalendar with given Id({request.CalendarId})");

            calendar.AddHoliday(new Holiday(
                request.Date, request.IsRecurring));

            await _workdayCalendarRepository
                .SaveAsync(calendar, cancellationToken);
        }
    }
}
