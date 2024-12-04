namespace LeadTimeCalculator.API.Domain.Shared.Contracts
{
    public interface IWorkdayCalendar
    {
        DateTime AddWorkingDays(DateTime startDate, double workingDayFractions);
        DateTime SubtractWorkingDays(DateTime endDate, double workingDayFractions);
    }
}
