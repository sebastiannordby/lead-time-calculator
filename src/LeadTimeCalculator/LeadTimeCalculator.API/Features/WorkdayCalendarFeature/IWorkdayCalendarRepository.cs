﻿using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature
{
    public interface IWorkdayCalendarRepository
    {
        Task<WorkdayCalendar> CreateAsync(
            TimeSpan defaultWorkdayStartTime,
            TimeSpan defaultWorkdayEndTime,
            CancellationToken cancellationToken = default);

        Task SaveAsync(
            WorkdayCalendar calendar,
            CancellationToken cancellationToken = default);
    }
}
