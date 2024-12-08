using FluentValidation;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.Queries.GetCalendars;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.AddExceptionDay;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.AddHoliday;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.CalculateLeadTime;
using LeadTimeCalculator.Production.Application.WorkdayCalendarFeature.UseCases.CreateCalendar;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddExceptionDay;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.AddHoliday;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CalculateLeadTime;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.CreateCalendar;
using LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.Production.Application.WorkdayCalendarFeature
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
