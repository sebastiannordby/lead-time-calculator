namespace LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models
{
    public class Holiday
    {
        public DateTime Date { get; }
        public bool IsRecurring { get; }

        public Holiday(DateTime date, bool isRecurring)
        {
            Date = date.Date;
            IsRecurring = isRecurring;
        }

        public bool Matches(DateTime date)
        {
            return IsRecurring ? Date.Month == date.Month && Date.Day == date.Day : Date.Date == date.Date;
        }
    }
}