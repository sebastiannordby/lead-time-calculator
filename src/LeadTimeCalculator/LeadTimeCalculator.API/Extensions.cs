using LeadTimeCalculator.API.Application.ProductionScheduleFeature.Repositories;
using LeadTimeCalculator.API.Application.Repositories.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Repositories;
using LeadTimeCalculator.API.Repositories;

namespace LeadTimeCalculator.API.Infrastructure
{
    internal static class Extensions
    {
        internal static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IWorkdayCalendarRepository, InMemoryWorkdayCalendarRepository>()
                .AddSingleton<IProducableProductRepository, InMemoryProducableProductRepository>();
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

        internal static void AddProductionScheduleFeatureEndpoints(
            this RouteGroupBuilder builder)
        {
            var productionScheduleGroup = builder
                .MapGroup("/production-schedule");

            var producableProductGroup = productionScheduleGroup
                .MapGroup("/producable-product");

            producableProductGroup.MapPost(
                string.Empty,
                ProductionScheduleEndpoints.ProducableProduct.AddProduct);
        }
    }
}
