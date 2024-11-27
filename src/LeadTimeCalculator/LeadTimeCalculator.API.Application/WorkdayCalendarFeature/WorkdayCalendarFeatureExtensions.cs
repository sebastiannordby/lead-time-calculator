using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature
{
    public static class WorkdayCalendarFeatureExtensions
    {
        public static IServiceCollection AddWorkdayCalendarApplicationFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<AddWorkdayCalendarExceptionDayRequestHandler>()
                .AddTransient<AddWorkdayCalendarHolidayRequestHandler>()
                .AddTransient<CalculateLeadTimeWorkdaysRequestHandler>()
                .AddTransient<CreateWorkdayCalendarRequestHandler>()
                .AddTransient<GetWorkdayCalendarsRequestHandler>();
        }
    }
}
