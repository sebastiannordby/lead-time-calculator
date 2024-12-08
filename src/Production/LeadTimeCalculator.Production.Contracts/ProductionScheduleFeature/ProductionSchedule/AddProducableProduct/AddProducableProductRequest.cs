namespace LeadTimeCalculator.Production.Constracts.ProductionScheduleFeature.ProductionSchedule.AddProducableProduct
{
    public sealed record AddProducableProductRequest(
        string ProductName,
        string ProductType,
        double WorkdaysToProduce,
        IEnumerable<string> Parts
    );
}
