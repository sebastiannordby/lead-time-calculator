﻿using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Application.Repositories.WorkdayCalendarFeature
{
    public interface IWorkdayCalendarRepository
    {
        Task<int> GetNewCalendarNumberAsync(
            CancellationToken cancellationToken = default);

        Task SaveAsync(
            WorkdayCalendar calendar,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<WorkdayCalendar>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<WorkdayCalendar?> FindAsync(
            int calendarId,
            CancellationToken cancellationToken);
    }
}
