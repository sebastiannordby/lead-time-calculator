
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.UseCases;
using LeadTimeCalculator.API.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature
{
    internal sealed class WorkdayCalendarEndpoints
    {
        internal static async Task<Results<Ok<CreateWorkdayCalendarResponse>, BadRequest<string>>> CreateCalendar(
            [FromBody] CreateWorkdayCalendarRequest request,
            [FromServices] CreateWorkdayCalendarRequestHandler requestHandler,
            CancellationToken cancellationToken)
        {
            try
            {
                var createCalendarResponse = await requestHandler
                    .HandleAsync(request, cancellationToken);

                return TypedResults.Ok(createCalendarResponse);
            }
            catch (DomainException ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
