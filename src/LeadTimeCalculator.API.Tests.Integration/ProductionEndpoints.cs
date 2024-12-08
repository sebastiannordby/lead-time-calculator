using LeadTimeCalculator.API.Tests.Integration.Shared;

namespace LeadTimeCalculator.API.Tests.Integration
{
    internal class ProductionEndpoints
    {
        internal ProductionWorkdayCalendarEndpoints Calendar { get; }
        internal ProductionScheduleEndpoints Schedule { get; }

        public ProductionEndpoints(HttpClient httpClient)
        {
            Calendar = new(httpClient);
            Schedule = new(httpClient);
        }
    }
}
