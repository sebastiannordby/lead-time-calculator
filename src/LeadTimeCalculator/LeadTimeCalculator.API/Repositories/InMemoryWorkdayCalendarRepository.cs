using LeadTimeCalculator.API.Application.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

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
            _workdayCalendars.Add(calendar);

            return await Task.FromResult(calendar);
        }

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

        public async Task SaveAsync(
            WorkdayCalendar calendar,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
