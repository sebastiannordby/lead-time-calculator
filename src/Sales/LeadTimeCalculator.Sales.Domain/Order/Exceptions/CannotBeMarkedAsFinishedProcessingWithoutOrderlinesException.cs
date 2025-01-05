namespace LeadTimeCalculator.Sales.Domain.Order.Exceptions
{
    public class CannotBeMarkedAsFinishedProcessingWithoutOrderlinesException : Exception
    {
        public CannotBeMarkedAsFinishedProcessingWithoutOrderlinesException()
            : base("Order cannot be marked as finished processing without order lines.")
        {
        }
    }
}
