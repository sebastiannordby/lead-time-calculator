namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models
{
    public sealed class ProductionTime
    {
        public double Workdays { get; }

        public ProductionTime(double workdays)
        {
            if (workdays <= 0)
                throw new ArgumentException("Production time in workdays must be greater than zero.", nameof(workdays));

            Workdays = workdays;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ProductionTime other)
                return false;

            return Workdays.Equals(other.Workdays);
        }

        public override int GetHashCode()
        {
            return Workdays.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Workdays} workdays";
        }
    }
}
