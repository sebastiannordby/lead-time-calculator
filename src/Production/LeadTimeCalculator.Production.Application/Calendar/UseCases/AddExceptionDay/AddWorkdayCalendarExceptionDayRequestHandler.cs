﻿using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Domain.Models.Calendar;

namespace LeadTimeCalculator.Production.Application.Calendar.UseCases.AddExceptionDay
{
    public sealed class AddWorkdayCalendarExceptionDayRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<AddWorkdayCalendarExceptionDayRequest> _requestValidator;

        public AddWorkdayCalendarExceptionDayRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<AddWorkdayCalendarExceptionDayRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task HandleAsync(
            AddWorkdayCalendarExceptionDayRequest request,
            CancellationToken cancellationToken)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

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
