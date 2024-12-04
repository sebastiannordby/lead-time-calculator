using FluentValidation;
using LeadTimeCalculator.API.Application.ProductionScheduleFeature.AddProduct;
using LeadTimeCalculator.API.Constracts.ProductionScheduleFeature.AddProducableProduct;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.API.Application.ProductionScheduleFeature
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
