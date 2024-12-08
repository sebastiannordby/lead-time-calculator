namespace LeadTimeCalculator.Production.Domain.Models.Order
{
    public sealed record ProductionOrderPart(
        string Name,
        DateTime? ExpectedArrivalDate);
}
