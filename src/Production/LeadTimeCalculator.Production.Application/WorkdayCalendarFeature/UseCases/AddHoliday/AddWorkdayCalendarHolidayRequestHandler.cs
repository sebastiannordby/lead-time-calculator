﻿using FluentValidation;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.Contracts;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.Production.Domain.Models.WorkdayCalendar;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.AddHoliday
{
    public class AddWorkdayCalendarHolidayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<AddWorkdayCalendarHolidayRequest> _requestValidator;

        public AddWorkdayCalendarHolidayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<AddWorkdayCalendarHolidayRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task HandleAsync(
            AddWorkdayCalendarHolidayRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

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
