using FluentValidation;
using LeadTimeCalculator.API.Constracts.ProductionScheduleFeature.AddProducableProduct;

namespace LeadTimeCalculator.API.Application.ProductionScheduleFeature.UseCases.AddProduct
{
    internal sealed class AddProducableProductRequestValidator
        : AbstractValidator<AddProducableProductRequest>
    {
        public AddProducableProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty();
            RuleFor(x => x.WorkdaysToProduce)
                .GreaterThan(0);
            RuleFor(x => x.ProductType)
                .NotEmpty();
        }
    }
}
