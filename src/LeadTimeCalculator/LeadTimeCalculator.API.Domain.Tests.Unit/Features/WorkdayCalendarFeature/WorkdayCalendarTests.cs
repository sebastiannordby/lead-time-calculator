using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

namespace LeadTimeCalculator.API.Tests.Unit.Features.WorkdayCalendarFeature
{
    /// <summary>
    /// Unit tests for calculating production and shipping dates with workday calendars.
    /// Covers scenarios with holidays, weekends, and exception days.
    /// Tests cleaned up by ChatGPT.
    /// </summary>
    public class WorkdayCalendarTests
    {
        /// <summary>
        /// Scenario: Calculate the production end date across a weekend.
        /// Given: Start date = 2024-11-25 08:00, 5 workdays needed, no holidays or exception days.
        /// When: Calculating the finish date.
        /// Then: The production should finish on 2024-12-03 12:00.
        /// </summary>
        [Fact]
        public void GivenFiveWorkdaysAndNoHolidays_WhenCalculatingFinishDate_ThenSkipsWeekends()
        {
            // Given
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(12)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(12)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
            };
            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());
            var receivePartsDayAndStartProduction = DateTime.Parse("2024-11-25 08:00");
            var workDaysToFinishProduct = 5;

            // When
            var result = workdayCalendar
                .CalculateDateForProductionFinished(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-12-03 12:00");
            Assert.Equal(expectedDate, result);
        }

        /// <summary>
        /// Scenario: Skip holidays during workday calculation.
        /// Given: Start date = 2024-11-22 09:00, 3 workdays needed, 1 holiday = 2024-11-25.
        /// When: Calculating the finish date.
        /// Then: The production should finish on 2024-11-28 09:00.
        /// </summary>
        [Fact]
        public void GivenAHolidayInTheMiddleOfProduction_WhenCalculatingFinishDate_ThenSkipsHoliday()
        {
            // Given
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
            };
            var holidays = new List<Holiday>
            {
                new Holiday(DateTime.Parse("2024-11-25"), isRecurring: false),
            };
            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);
            var receivePartsDayAndStartProduction = DateTime.Parse("2024-11-22 09:00");
            var workDaysToFinishProduct = 3;

            // When
            var result = workdayCalendar
                .CalculateDateForProductionFinished(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-11-28 09:00");
            Assert.Equal(expectedDate, result);
        }

        /// <summary>
        /// Scenario: Fractional workdays are added within a single valid working day.
        /// Given: Start date = 2024-03-01 09:00, 0.5 workdays needed, no holidays or exception days.
        /// When: Calculating the finish time.
        /// Then: The production should finish on 2024-03-01 13:00.
        /// </summary>
        [Fact]
        public void GivenFractionalWorkdays_WhenCalculatingFinishTime_ThenRespectsDailyWorkingHours()
        {
            // Given
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
            };
            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());
            var receivePartsDayAndStartProduction = DateTime.Parse("2024-03-01 09:00");
            var workDaysToFinishProduct = 0.5;

            // When
            var result = workdayCalendar
                .CalculateDateForProductionFinished(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, result);
        }
    }
}
