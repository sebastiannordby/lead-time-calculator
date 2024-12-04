using FluentValidation;
using LeadTimeCalculator.API.Application.Repositories.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases.GetCalendars
{
    public sealed class GetWorkdayCalendarsRequestHandler
    {
        private readonly IWorkdayCalendarRepository _workdayCalendarRepository;
        private readonly IValidator<GetWorkdayCalendarsRequest> _requestValidator;

        public GetWorkdayCalendarsRequestHandler(
            IWorkdayCalendarRepository workdayCalendarRepository,
            IValidator<GetWorkdayCalendarsRequest> requestValidator)
        {
            _workdayCalendarRepository = workdayCalendarRepository;
            _requestValidator = requestValidator;
        }

        public async Task<GetWorkdayCalendarsResponse> HandleAsync(
            GetWorkdayCalendarsRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            var calendars = await _workdayCalendarRepository
                .GetAllAsync(cancellationToken);
            var calendarDetailViews = calendars.Select(calendar =>
            {
                var exceptionDays = calendar.ExceptionDays
                    .Select(exceptionDay => new CalendarDetailedView.ExceptionDayView()
                    {
                        Date = exceptionDay.Date,
                        EndTime = exceptionDay.EndTime,
                        StartTime = exceptionDay.StartTime
                    });

                var holidays = calendar.Holidays
                    .Select(holiday => new CalendarDetailedView.HolidayView()
                    {
                        Date = holiday.Date,
                        IsRecurring = holiday.IsRecurring
                    });

                var workingDays = calendar.WorkWeek
                    .GetWorkingWeek()
                    .Select(day => new CalendarDetailedView.DefaultWorkingDayView()
                    {
                        DayOfWeek = day.Key,
                        EndTime = day.Value.EndTime,
                        StartTime = day.Value.StartTime
                    });

                return new CalendarDetailedView()
                {
                    Id = calendar.Id,
                    ExceptionDays = exceptionDays.ToArray(),
                    Holidays = holidays.ToArray(),
                    DefaultWorkingDays = workingDays.ToArray()
                };
            });

            var response = new GetWorkdayCalendarsResponse(calendarDetailViews);

            return response;
        }
    }
}
