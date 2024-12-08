namespace LeadTimeCalculator.Production.Contracts.Calendar.GetCalendars
{
    public sealed record GetWorkdayCalendarsRequest
    {
        public int? Take { get; init; }
        public int? Skip { get; init; }
    }
}
