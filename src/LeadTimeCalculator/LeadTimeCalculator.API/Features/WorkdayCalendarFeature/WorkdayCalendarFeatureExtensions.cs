using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature
{
    internal static class WorkdayCalendarFeatureExtensions
    {
        internal static IServiceCollection AddWorkdayCalendarFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<CreateWorkdayCalendarRequestHandler>()
                .AddTransient<GetWorkdayCalendarsRequestHandler>()
                .AddTransient<AddWorkdayCalendarHolidayRequestHandler>()
                .AddTransient<AddWorkdayCalendarExceptionDayRequestHandler>()
                .AddTransient<CalculateLeadTimeWorkdaysRequestHandler>();
        }

        internal static void AddWorkdayCalendarFeatureEndpoints(
            this RouteGroupBuilder builder)
        {
            var workdayCalendarGroup = builder
                .MapGroup("/workday-calendar");

            workdayCalendarGroup
                .MapPost(string.Empty, WorkdayCalendarEndpoints.CreateCalendar);

            workdayCalendarGroup
                .MapPost("/add-holiday", WorkdayCalendarEndpoints.AddHoliday);

            workdayCalendarGroup
                .MapPost("/add-exception-day", WorkdayCalendarEndpoints.AddExceptionDay);

            workdayCalendarGroup
                .MapPost("/calculate-lead-time-workdays", WorkdayCalendarEndpoints.CalculateLeadTimeWorkdays);

            workdayCalendarGroup
                .MapPost("/list", WorkdayCalendarEndpoints.GetWorkdayCalendars);
        }
    }
}
