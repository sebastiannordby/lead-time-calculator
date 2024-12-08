using FluentValidation;
using LeadTimeCalculator.Production.Application.Calendar.Queries.GetCalendars;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.AddExceptionDay;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.AddHoliday;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateTimeBackward;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculculateTimeForward;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CreateCalendar;
using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.UseCases.AddProduct;
using LeadTimeCalculator.Production.Contracts.Calendar.AddExceptionDay;
using LeadTimeCalculator.Production.Contracts.Calendar.AddHoliday;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars;
using LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTimeCalculator.Production.Application
{
    public static class ProductionScheduleFeatureExtensions
    {
        public static IServiceCollection AddProductionApplicationFeature(
            this IServiceCollection services)
        {
            return services
                .AddScheduleFeature()
                .AddCalendarFeature();
        }

        internal static IServiceCollection AddScheduleFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<CalculateWorkdayCalendarTimeForwardRequestHandler>()
                .AddTransient<IValidator<CalculateWorkdayCalendarTimeForwardRequest>, CalculateWorkdayCalendarTimeForwardRequestValidator>()
                .AddTransient<AddProducableProductRequestHandler>()
                .AddTransient<IValidator<AddProducableProductRequest>, AddProducableProductRequestValidator>();
        }

        internal static IServiceCollection AddCalendarFeature(
            this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<AddWorkdayCalendarExceptionDayRequest>, AddWorkdayCalendarExceptionDayRequestValidator>()
                .AddTransient<AddWorkdayCalendarExceptionDayRequestHandler>()
                .AddTransient<IValidator<AddWorkdayCalendarHolidayRequest>, AddWorkdayCalendarHolidayRequestValidator>()
                .AddTransient<AddWorkdayCalendarHolidayRequestHandler>()
                .AddTransient<IValidator<CalculateWorkdayCalendarTimeBackwardRequest>, CalculateWorkdayCalendarTimeBackwardRequestValidator>()
                .AddTransient<CalculateWorkdayCalendarTimeBackwardRequestHandler>()
                .AddTransient<IValidator<CreateWorkdayCalendarRequest>, CreateWorkdayCalendarRequestValidator>()
                .AddTransient<CreateWorkdayCalendarRequestHandler>()
                .AddTransient<IValidator<GetWorkdayCalendarsRequest>, GetWorkdayCalendarsRequestValidator>()
                .AddTransient<GetWorkdayCalendarsRequestHandler>();
        }
    }
}
