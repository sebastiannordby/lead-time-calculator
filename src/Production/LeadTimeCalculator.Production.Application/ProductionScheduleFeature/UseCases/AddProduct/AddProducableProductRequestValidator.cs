using FluentValidation;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.ProductionSchedule.AddProducableProduct;

namespace LeadTimeCalculator.Production.Application.ProductionScheduleFeature.UseCases.AddProduct
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
