using FluentValidation;
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
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.Production.Application.Calendar
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
