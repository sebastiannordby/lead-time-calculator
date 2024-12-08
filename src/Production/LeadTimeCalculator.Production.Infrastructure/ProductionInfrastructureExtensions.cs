using LeadTimeCalculator.API.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Repositories;
using LeadTimeCalculator.API.Repositories;
using LeadTimeCalculator.Production.Application.Calendar.Queries.Contracts;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.Repositories;
using LeadTimeCalculator.Production.Infrastructure.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.Production.Infrastructure
{
    public static class ProductionInfrastructureExtensions
    {
        public static IServiceCollection AddProductionInfrastructure(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IQueryWorkdayCalendarDetailedView, InMemoryQueryWorkdayCalendarDetailedView>()
                .AddSingleton<IWorkdayCalendarRepository, InMemoryWorkdayCalendarRepository>()
                .AddSingleton<IProducableProductRepository, InMemoryProducableProductRepository>()
                .AddSingleton<InMemoryWorkdayCalendarRepository>();
        }

        public static void AddProductionEndpoints(
            this RouteGroupBuilder apiGroupBuilder)
        {
            var productionGroupBuilder = apiGroupBuilder
                .MapGroup("/production");

            productionGroupBuilder
                .AddWorkdayCalendarFeatureEndpoints();

            productionGroupBuilder
                .AddProductionScheduleFeatureEndpoints();
        }

        internal static void AddWorkdayCalendarFeatureEndpoints(
            this RouteGroupBuilder builder)
        {
            var workdayCalendarGroup = builder
                .MapGroup("/calendar");

            workdayCalendarGroup.MapPost(
                string.Empty,
                WorkdayCalendarEndpoints.CreateCalendar);

            workdayCalendarGroup.MapPost(
                "/add-holiday",
                WorkdayCalendarEndpoints.AddHoliday);

            workdayCalendarGroup.MapPost(
                "/add-exception-day",
                WorkdayCalendarEndpoints.AddExceptionDay);

            workdayCalendarGroup.MapPost(
                "/list",
                WorkdayCalendarEndpoints.GetWorkdayCalendars);

            workdayCalendarGroup.MapPost(
                "/calculate-time-backward",
                WorkdayCalendarEndpoints.CalculateWorkdayCalendarTimeBackward);

            workdayCalendarGroup.MapPost(
                "/calculate-time-forward",
                WorkdayCalendarEndpoints.CalculateWorkdayCalendarTimeForward);
        }

        internal static void AddProductionScheduleFeatureEndpoints(
            this RouteGroupBuilder builder)
        {
            var productionScheduleGroup = builder
                .MapGroup("/schedule");

            var producableProductGroup = productionScheduleGroup
                .MapGroup("/producable-product");

            producableProductGroup.MapPost(
                string.Empty,
                ProductionScheduleEndpoints.ProducableProduct.AddProduct);
        }
    }
}
