using LeadTimeCalculator.API.Features.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Infrastructure.Repositories
{
    /// <summary>
    /// In memory collection of workday calendars.
    /// </summary>
    public class InMemoryWorkdayCalendarRepository : IWorkdayCalendarRepository
    {
        private readonly List<WorkdayCalendar> _workdayCalendars = new();

        public async Task<WorkdayCalendar> CreateAsync(
            TimeSpan defaultWorkdayStartTime,
            TimeSpan defaultWorkdayEndTime,
            CancellationToken cancellationToken = default)
        {
            var calendarNumberSeries = _workdayCalendars
                .Select(x => x.Id);
            var calendarId = !calendarNumberSeries.Any()
                ? 1 : calendarNumberSeries.Max() + 1;

            var calendar = new WorkdayCalendar(
                calendarId,
                defaultWorkdayStartTime,
                defaultWorkdayEndTime);

            return await Task.FromResult(calendar);
        }

        public async Task SaveAsync(
            WorkdayCalendar calendar,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
