namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature
{
    public sealed class ProductType
    {
        public string Name { get; }

        public ProductType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product type name cannot be null or empty.", nameof(name));

            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ProductType other)
                return false;

            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase); // Case-insensitive comparison
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
