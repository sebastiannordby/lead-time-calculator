using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Tests.Unit.Features.WorkdayCalendarFeature
{
    public class WorkdayCalendarTests
    {
        /// <summary>
        /// 1. Adding Working Days Across Weekends
        /// Input: Start datetime = 2024-03-01 09:00, Add 5 working days
        /// Expected Output: 2024-03-08 09:00
        /// </summary>
        [Fact]
        public void AddingWorkingDaysAcrossWeekends()
        {
            // Arrange
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>()
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

            var startDate = DateTime.Parse("2024-03-01 09:00");

            // Act
            var result = workdayCalendar.AddFractionalWorkingDays(startDate, 5);

            // Assert
            var expectedDate = DateTime.Parse("2024-03-08 09:00");
            Assert.Equal(expectedDate, result);
        }

        /// <summary>
        /// 2. Skipping Holidays During Working Day Calculation
        /// Input: Start datetime = 2024-12-22 09:00, Add 3 working days
        /// Set single holiday = 2024-12-25
        /// Expected Output: 2024-12-27 09:00
        /// </summary>
        [Fact]
        public void SkippingHolidaysDuringWorkingDayCalculation()
        {
            // Arrange
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>()
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
            };

            var holidays = new List<Holiday>
            {
                new Holiday(DateTime.Parse("2024-12-25"), isRecurring: false),
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var startDate = DateTime.Parse("2024-12-22 09:00");

            // Act
            var result = workdayCalendar.AddFractionalWorkingDays(startDate, 3);

            // Assert
            var expectedDate = DateTime.Parse("2024-12-27 09:00");
            Assert.Equal(expectedDate, result);
        }

        /// <summary>
        /// 4. Exception Day Spilling Over to Next Working Day
        /// Input: Start datetime = 2024-03-01 12:00, Add 0.5 working days
        /// Exception Day: 2024-03-01 with hours 11:00 to 14:00
        /// Expected Output: 2024-03-04 10:00
        /// </summary>
        [Fact]
        public void FractionalWorkingDayAdditionWithinSingleDay()
        {
            // Arrange
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>()
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

            var startDate = DateTime.Parse("2024-03-01 09:00");

            // Act
            var result = workdayCalendar.AddFractionalWorkingDays(startDate, 0.5);

            // Assert
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, result);
        }


        /// <summary>
        /// 4. Produce what we can on exception day
        /// if it s
        /// </summary>
        [Fact]
        public void ExceptionDaySpillingOverToNextWorkingDay()
        {
            // Arrange
            var defaultWorkingHours = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) }
            };

            var exceptionDays = new List<ExceptionDay>
            {
                // 3 hours available
                // But only 2 hours available after startDate
                new ExceptionDay(new DateTime(2024, 3, 1), TimeSpan.FromHours(11), TimeSpan.FromHours(14))
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingHours,
                Enumerable.Empty<Holiday>());
            exceptionDays.ForEach(workdayCalendar.AddExceptionDay);

            var startDate = DateTime.Parse("2024-03-01 12:00"); // Start within exception hours on 01 March 2024 after production

            // Act
            var result = workdayCalendar.AddFractionalWorkingDays(startDate, 0.5); // Add 4 hours (0.5 working days)

            // Assert
            var expectedDate = DateTime.Parse("2024-03-04 10:00"); // Spillover into Monday, 04 March 2024
            Assert.Equal(expectedDate, result);
        }

    }
}
