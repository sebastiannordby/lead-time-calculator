using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;
using LeadTimeCalculator.API.Shared.Exceptions;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases
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
                throw new DomainException($"No WorkdayCalendar with given Id({request.CalendarId})");

            calendar.AddHoliday(new Holiday(
                request.Date, request.IsRecurring));

            await _workdayCalendarRepository
                .SaveAsync(calendar, cancellationToken);
        }
    }
}
