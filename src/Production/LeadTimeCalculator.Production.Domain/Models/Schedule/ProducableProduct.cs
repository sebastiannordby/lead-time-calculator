using LeadTimeCalculator.Production.Domain.Exceptions;

namespace LeadTimeCalculator.Production.Domain.Models.Schedule
{
    public sealed class ProducableProduct
    {
        private readonly List<ProducableProductPart> _parts;

        public string Name { get; }
        public ProductionTime ProductionTime { get; }
        public ProductType ProductType { get; }
        public IReadOnlyCollection<ProducableProductPart> Parts => _parts;

        public ProducableProduct(
            string name,
            ProductionTime productionTime,
            ProductType productType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

            Name = name;
            ProductType = productType ?? throw new ArgumentNullException(nameof(productType));
            ProductionTime = productionTime ?? throw new ArgumentException(nameof(productionTime));
            _parts = new List<ProducableProductPart>();
        }

        public void AddPart(
            ProducableProductPart part)
        {
            if (part is null)
                throw new ArgumentNullException(nameof(part));
            if (_parts.Contains(part))
                throw new ProducableProductAddExistingPartException("Product already contains part");

            _parts.Add(part);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ProducableProduct other)
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
