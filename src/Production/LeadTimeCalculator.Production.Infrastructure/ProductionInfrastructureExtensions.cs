using LeadTimeCalculator.API.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Endpoints;
using LeadTimeCalculator.API.Infrastructure.Repositories;
using LeadTimeCalculator.API.Repositories;
using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.Repositories;
using LeadTimeCalculator.Production.Application.Calendar.Queries.Contracts;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
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

        public static RouteGroupBuilder AddProductionEndpoints(
            this RouteGroupBuilder apiGroupBuilder)
        {
            apiGroupBuilder
                .AddWorkdayCalendarFeatureEndpoints();

            apiGroupBuilder
                .AddProductionScheduleFeatureEndpoints();

            return apiGroupBuilder;
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
