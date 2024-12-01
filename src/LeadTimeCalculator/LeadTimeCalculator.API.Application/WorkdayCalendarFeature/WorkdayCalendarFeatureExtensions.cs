﻿using FluentValidation;
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
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.API.Application.WorkdayCalendarFeature
{
    public static class WorkdayCalendarFeatureExtensions
    {
        public static IServiceCollection AddWorkdayCalendarApplicationFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<AddWorkdayCalendarExceptionDayRequest>, AddWorkdayCalendarExceptionDayRequestValidator>()
                .AddTransient<AddWorkdayCalendarExceptionDayRequestHandler>()
                .AddTransient<IValidator<AddWorkdayCalendarHolidayRequest>, AddWorkdayCalendarHolidayRequestValidator>()
                .AddTransient<AddWorkdayCalendarHolidayRequestHandler>()
                .AddTransient<IValidator<CalculateLeadTimeWorkdaysRequest>, CalculateLeadTimeWorkdaysRequestValidator>()
                .AddTransient<CalculateLeadTimeWorkdaysRequestHandler>()
                .AddTransient<IValidator<CreateWorkdayCalendarRequest>, CreateWorkdayCalendarRequestValidator>()
                .AddTransient<CreateWorkdayCalendarRequestHandler>()
                .AddTransient<IValidator<GetWorkdayCalendarsRequest>, GetWorkdayCalendarsRequestValidator>()
                .AddTransient<GetWorkdayCalendarsRequestHandler>();
        }
    }
}