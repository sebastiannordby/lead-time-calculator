namespace LeadTimeCalculator.API.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string? message) : base(message)
        {
        }
    }
}
