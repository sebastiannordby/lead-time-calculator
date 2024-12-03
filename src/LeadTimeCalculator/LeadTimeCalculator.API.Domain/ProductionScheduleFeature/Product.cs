namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature
{
    public sealed class Product
    {
        public string Name { get; }
        public ProductionTime ProductionTime { get; }
        public ProductType ProductType { get; }

        public Product(
            string name,
            ProductionTime productionTime,
            ProductType productType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

            Name = name;
            ProductType = productType ?? throw new ArgumentNullException(nameof(productType));
            ProductionTime = productionTime ?? throw new ArgumentException(nameof(productionTime));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Product other)
                return false;

            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   ProductionTime.Equals(other.ProductionTime) &&
                   ProductType.Equals(other.ProductType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(Name),
                ProductionTime,
                ProductType);
        }

        public override string ToString()
        {
            return $"{Name} (Type: {ProductType}, Production Time: {ProductionTime} workdays)";
        }
    }

}
