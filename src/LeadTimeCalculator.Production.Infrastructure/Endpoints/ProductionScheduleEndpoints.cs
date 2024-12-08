using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.UseCases.AddProduct;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.ProductionSchedule.AddProducableProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

namespace LeadTimeCalculator.API.Endpoints
{
    public class ProductionScheduleEndpoints
    {
        public static class ProducableProduct
        {
            internal static async Task<Results<Ok, BadRequest<string>>> AddProduct(
                [FromBody] AddProducableProductRequest request,
                [FromServices] AddProducableProductRequestHandler requestHandler,
                CancellationToken cancellationToken)
            {
                try
                {
                    await requestHandler
                        .HandleAsync(request, cancellationToken);

                    return TypedResults.Ok();
                }
                catch (ValidationException ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
            }
        }
    }
}
