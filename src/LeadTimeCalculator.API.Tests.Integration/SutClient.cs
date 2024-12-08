using LeadTimeCalculator.API.Tests.Integration.Shared;

namespace LeadTimeCalculator.API.Tests.Integration
{
    internal class SutClient
    {
        internal WorkdayCalendarEndpoints WorkdayCalendar { get; }
        internal ProductionScheduleEndpoints ProductionSchedule { get; }

        private readonly HttpClient _httpClient;

        internal SutClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            WorkdayCalendar = new(_httpClient);
            ProductionSchedule = new(_httpClient);
        }
    }
}
