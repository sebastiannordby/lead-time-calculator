using FluentValidation;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.AddExceptionDay;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.AddHoliday;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.CalculateLeadTime;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.CreateCalendar;
using LeadTimeCalculator.API.Application.WorkdayCalendarFeature.GetCalendars;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.API.Constracts.WorkdayCalendar.GetCalendars;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LeadTimeCalculator.API.Infrastructure.Endpoints
{
    internal sealed class WorkdayCalendarEndpoints
    {
        internal static async Task<Results<Ok, BadRequest<string>>> AddExceptionDay(
            [FromBody] AddWorkdayCalendarExceptionDayRequest request,
            [FromServices] AddWorkdayCalendarExceptionDayRequestHandler requestHandler,
            CancellationToken cancellationToken)
        {
            try
            {
                await requestHandler
                    .HandleAsync(request, cancellationToken);

                return TypedResults.Ok();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ValidationException)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        internal static async Task<Results<Ok, BadRequest<string>>> AddHoliday(
            [FromBody] AddWorkdayCalendarHolidayRequest request,
            [FromServices] AddWorkdayCalendarHolidayRequestHandler requestHandler,
            CancellationToken cancellationToken)
        {
            try
            {
                await requestHandler
                    .HandleAsync(request, cancellationToken);

                return TypedResults.Ok();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ValidationException)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        internal static async Task<Results<Ok<CalculateLeadTimeWorkdaysResponse>, BadRequest<string>>> CalculateLeadTimeWorkdays(
            [FromBody] CalculateLeadTimeWorkdaysRequest request,
            [FromServices] CalculateLeadTimeWorkdaysRequestHandler requestHandler,
            CancellationToken cancellationToken)
        {
            try
            {
                var calculateLeadTimeResponse = await requestHandler
                    .HandleAsync(request, cancellationToken);

                return TypedResults.Ok(calculateLeadTimeResponse);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ValidationException)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

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
            catch (Exception ex) when (ex is ArgumentException || ex is ValidationException)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        internal static async Task<Results<Ok<GetWorkdayCalendarsResponse>, BadRequest<string>>> GetWorkdayCalendars(
            [FromBody] GetWorkdayCalendarsRequest request,
            [FromServices] GetWorkdayCalendarsRequestHandler requestHandler,
            CancellationToken cancellationToken)
        {
            try
            {
                var getWorkdayCalendarsResponse = await requestHandler
                    .HandleAsync(request, cancellationToken);

                return TypedResults.Ok(getWorkdayCalendarsResponse);

            }
            catch (Exception ex) when (ex is ArgumentException || ex is ValidationException)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
