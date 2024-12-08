namespace LeadTimeCalculator.Production.Domain.Models.Schedule
{
    public sealed record ProductionOrderPart(
        string Name,
        DateTime? ExpectedArrivalDate);
}
