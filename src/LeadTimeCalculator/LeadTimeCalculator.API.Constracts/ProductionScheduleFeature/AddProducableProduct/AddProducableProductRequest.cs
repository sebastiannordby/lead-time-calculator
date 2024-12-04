namespace LeadTimeCalculator.API.Constracts.ProductionScheduleFeature.AddProducableProduct
{
    public sealed record AddProducableProductRequest(
        string ProductName,
        string ProductType,
        double WorkdaysToProduce,
        IEnumerable<string> Parts
    );
}
