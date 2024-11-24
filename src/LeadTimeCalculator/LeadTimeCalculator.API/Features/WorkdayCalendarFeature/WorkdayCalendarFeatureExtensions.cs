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
                .AddTransient<GetWorkdayCalendarsRequestHandler>();
        }

        internal static void AddWorkdayCalendarFeatureEndpoints(
            this RouteGroupBuilder builder)
        {
            var workdayCalendarGroup = builder
                .MapGroup("/workday-calendar");

            workdayCalendarGroup
                .MapPost(string.Empty, WorkdayCalendarEndpoints.CreateCalendar);

            workdayCalendarGroup
                .MapPost("/list", WorkdayCalendarEndpoints.GetWorkdayCalendars);
        }
    }
}
