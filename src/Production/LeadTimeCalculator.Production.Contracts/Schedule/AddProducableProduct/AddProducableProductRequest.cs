namespace LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct
{
    public sealed record AddProducableProductRequest(
        string ProductName,
        string ProductType,
        double WorkdaysToProduce,
        IEnumerable<string> Parts
    );
}
