using LeadTimeCalculator.API.Application.Repositories.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Infrastructure.Repositories
{
    /// <summary>
    /// In memory collection of workday calendars.
    /// </summary>
    public class InMemoryWorkdayCalendarRepository : IWorkdayCalendarRepository
    {
        private readonly List<WorkdayCalendar> _workdayCalendars = new();

        public async Task<WorkdayCalendar?> FindAsync(
            int calendarId,
            CancellationToken cancellationToken)
        {
            var calendar = _workdayCalendars
                .FirstOrDefault(x => x.Id == calendarId);

            return await Task.FromResult(calendar);
        }

        public async Task<IReadOnlyCollection<WorkdayCalendar>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var workdayCalendars = _workdayCalendars
                .AsReadOnly();

            return await Task.FromResult(workdayCalendars);
        }

        public async Task<int> GetNewCalendarNumberAsync(
            CancellationToken cancellationToken = default)
        {
            var calendarNumberSeries = _workdayCalendars
                .Select(x => x.Id);
            var calendarId = !calendarNumberSeries.Any()
                ? 1 : calendarNumberSeries.Max() + 1;

            return await Task.FromResult(calendarId);
        }

        public async Task SaveAsync(
            WorkdayCalendar calendar,
            CancellationToken cancellationToken = default)
        {
            if (!_workdayCalendars.Any(x => x.Id == calendar.Id))
            {
                _workdayCalendars.Add(calendar);
            }

            await Task.CompletedTask;
        }
    }
}
