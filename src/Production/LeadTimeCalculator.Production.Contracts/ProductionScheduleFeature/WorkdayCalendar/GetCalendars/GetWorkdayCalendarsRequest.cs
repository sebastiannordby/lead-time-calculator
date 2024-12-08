namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.WorkdayCalendar.GetCalendars
{
    public sealed record GetWorkdayCalendarsRequest
    {
        public int? Take { get; init; }
        public int? Skip { get; init; }
    }
}
