
namespace LeadTimeCalculator.API.Constracts.WorkdayCalendar.CalculateLeadTime
{
    public sealed record CalculateLeadTimeWorkdaysRequest(
        int CalendarId,
        DateTime StartingDate,
        double WorkdaysAdjustment)
    {
        public async Task<object> HandleAsync(CalculateLeadTimeWorkdaysRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
