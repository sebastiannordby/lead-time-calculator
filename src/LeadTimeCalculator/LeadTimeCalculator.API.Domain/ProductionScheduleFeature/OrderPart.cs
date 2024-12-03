namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature
{
    public sealed record OrderPart(
        string Name,
        DateTime? ExpectedArrivalDate);
}
