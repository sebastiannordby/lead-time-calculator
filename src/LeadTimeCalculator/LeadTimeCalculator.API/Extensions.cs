using LeadTimeCalculator.API.Application.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Infrastructure.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Repositories;

namespace LeadTimeCalculator.API.Infrastructure
{
    internal static class Extensions
    {
        internal static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IWorkdayCalendarRepository, InMemoryWorkdayCalendarRepository>();
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
