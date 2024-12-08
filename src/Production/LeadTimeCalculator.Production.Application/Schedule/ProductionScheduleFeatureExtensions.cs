using FluentValidation;
using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.UseCases.AddProduct;
using LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.Production.Application.ProductionScheduleFeature
{
    public static class ProductionScheduleFeatureExtensions
    {
        public static IServiceCollection AddProductionScheduleApplicationFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<AddProducableProductRequestHandler>()
                .AddTransient<IValidator<AddProducableProductRequest>, AddProducableProductRequestValidator>();
        }
    }
}
