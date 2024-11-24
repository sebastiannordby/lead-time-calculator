using LeadTimeCalculator.API.Features.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Infrastructure.Repositories;

namespace LeadTimeCalculator.API.Infrastructure
{
    internal static class InfrastructureExtensions
    {
        internal static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IWorkdayCalendarRepository, InMemoryWorkdayCalendarRepository>();
        }
    }
}
