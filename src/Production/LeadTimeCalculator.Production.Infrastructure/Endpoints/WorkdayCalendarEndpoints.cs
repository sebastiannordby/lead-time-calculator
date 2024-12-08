﻿using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.Queries.GetCalendars;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.AddExceptionDay;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.AddHoliday;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateLeadTime;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateLeadTime;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;
using Microsoft.AspNetCore.Http;
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
