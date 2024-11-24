using LeadTimeCalculator.API.Features.WorkdayCalendarFeature.Models;
using System.Globalization;

namespace LeadTimeCalculator.API.Tests.Unit.Features.WorkdayCalendarFeature
{
    /// <summary>
    /// Tests in this class are named after the exercises given in the document.
    /// In a real life scenario i would most likely have a more BDD-approach for the tests,
    /// but given the available time i found this to be the easiest approach.
    /// </summary>
    public class TechnicalInterviewTests
    {
        /// <summary>
        /// 1. Basic Working Day Calculation
        /// Scenario: Calculate the result of adding a quarter of a working day to a given start datetime.
        /// Input: Start datetime = 24.05.2004 15:07, Add 0.25 working days, Workday start = 08:00, end = 16:00
        /// Expected Output: The resulting datetime should be the next day at 09:07, assuming the next day is a
        /// working day and not a holiday.
        /// </summary>
        [Fact]
        public void BasicWorkingDayCalculationTest()
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

            var date = DateTime.Parse("2004-05-24 15:07"); // 24.05.2004 15:07

            // Act
            var work = workdayCalendar
                .CalculateLeadTimeWorkdays(date, 0.25);

            // Assert
            var expectedDate = new DateTime(2004, 5, 25, 09, 7, 0); // 25.05.2004 09:07

            Assert.Equal(expectedDate, work);
        }

        /// <summary>
        /// 2. Midnight Boundary and Fractional Working Day
        /// Scenario: Calculate the result of adding half a working day to a start datetime that begins before the
        ///     working hours.
        /// Input: Start datetime = 24.05.2004 04:00, Add 0.5 working days, Workday start = 08:00, end = 16:00
        /// Expected Output: The resulting datetime should be the same day at 12:00 (noon), as half a working
        /// day from the start of working hours(08:00) leads to 12:00.
        /// </summary>
        [Fact]
        public void MidnightBoundaryandFractionalWorkingDayTests()
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

            var date = DateTime.Parse("2004-05-24 04:00"); // 24.05.2004 04:00

            // Act 
            var work = workdayCalendar
                .CalculateLeadTimeWorkdays(date, 0.5);

            // Assert
            var expectedDate = DateTime.Parse("2004-05-24 12:00"); // 24.05.2004 12:00

            Assert.Equal(expectedDate, work);
        }

        /// <summary>
        /// 3. Working Day Calculation with Negative Days
        /// Scenario: Calculate the result of subtracting 5.5 working days from a given start datetime.
        /// Input: Start datetime = 24.05.2004 18:05, Subtract 5.5 working days, Set recurring holiday = 17 May, 
        /// Set single holiday = 27 May 2004, Workday start = 08:00, end = 16:00
        /// Expected Output: The resulting datetime should be 14.05.2004 12:00. This calculation must account 
        /// for the non-working days(weekends) and the specified holidays.
        /// </summary>
        [Fact]
        public void WorkingDayCalculationwithNegativeDays()
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

            var holidays = new List<Holiday>()
            {
                new Holiday(DateTime.Parse("2004-05-27"), isRecurring: false),
                new Holiday(DateTime.Parse("2004-05-17"), isRecurring: true)
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var date = DateTime.Parse("2004-05-24 18:05"); // 24.05.2004 18:05

            // Act
            var work = workdayCalendar
                .CalculateLeadTimeWorkdays(date, -5.5);

            // Assert
            var expectedDate = DateTime.Parse("2004-05-14 12:00"); // 14.05.2004 12:00

            Assert.Equal(expectedDate, work);
        }

        /// <summary>
        /// These examples demonstrate how the system should handle various additions of working days to 
        ///  specific start datetimes, considering the start and end of the working day, weekends, and holidays.
        ///  Set recurring holiday = 17 May, Set single holiday = 27 May 2004, Workday start = 08:00, end = 16:00.
        ///      • Example 1:
        ///      o Input: 24-05-2004 19:03, Add 44.723656 working days,
        ///              o Expected Output: 27-07-2004 13:47
        ///      • Example 2:
        ///      o Input: 24-05-2004 18:03, Subtract 6.7470217 working days
        ///              o Expected Output: 13-05-2004 10:01
        ///      • Example 3:
        ///      o Input: 24-05-2004 08:03, Add 12.782709 working days
        ///              o Expected Output: 10-06-2004 14:18
        ///      • Example 4:
        ///      o Input: 24-05-2004 07:03, Add 8.276628 working days
        ///              o Expected Output: 04-06-2004 10:12
        /// </summary>
        [Theory]
        [InlineData("2004-05-24 19:03", 44.723656, "2004-07-27 13:47")] // Example 1
        [InlineData("2004-05-24 18:03", -6.7470217, "2004-05-13 10:01")] // Example 2
        [InlineData("2004-05-24 08:03", 12.782709, "2004-06-10 14:18")] // Example 3
        [InlineData("2004-05-24 07:03", 8.276628, "2004-06-04 10:12")]  // Example 4
        public void AdditionalCorrectResults(
            string inputDateStr, double workingHoursToAdd, string expectedDateStr)
        {
            DateTime inputDate = DateTime.ParseExact(inputDateStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            // Arrange
            var defaultWorkingDays = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>()
            {
                { DayOfWeek.Monday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Tuesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Wednesday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Thursday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
                { DayOfWeek.Friday, (TimeSpan.FromHours(8), TimeSpan.FromHours(16)) },
            };
            var holidays = new List<Holiday>()
            {
                new Holiday(DateTime.ParseExact("2004/05/27", "yyyy/MM/dd", CultureInfo.InvariantCulture), isRecurring: false),
                new Holiday(DateTime.ParseExact("2004/05/17", "yyyy/MM/dd", CultureInfo.InvariantCulture), isRecurring: true)
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            // Act
            var work = workdayCalendar
                .CalculateLeadTimeWorkdays(inputDate, workingHoursToAdd);

            // Assert
            Assert.Equal(expectedDate, work);
        }
    }
}
