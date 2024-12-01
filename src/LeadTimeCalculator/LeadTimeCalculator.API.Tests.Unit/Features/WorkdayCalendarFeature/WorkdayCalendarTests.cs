using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

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

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-03-01 09:00");
            var workDaysToFinishProduct = 5;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(receivePartsDayAndStartProduction, workDaysToFinishProduct);

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

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-12-22 09:00");
            var workDaysToFinishProduct = 3;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(receivePartsDayAndStartProduction, workDaysToFinishProduct);

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

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-03-01 09:00");
            var workDaysToFinishProduct = 0.5;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Assert
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, result);
        }


        /// <summary>
        /// 4. Produce what we can on exception day
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

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-03-01 12:00"); // Start within exception hours on 01 March 2024 after production
            var workDaysToFinishProduct = 0.5;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(receivePartsDayAndStartProduction, workDaysToFinishProduct); // Add 4 hours (0.5 working days)

            // Assert
            var expectedDate = DateTime.Parse("2024-03-04 10:00"); // Spillover into Monday, 04 March 2024
            Assert.Equal(expectedDate, result);
        }

        [Fact]
        public void ExpectedToShipInTheMorningButLimitedAvailableTimeAndHolidayAndExceptionDayPreceeding()
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
                // 4 hours available
                new ExceptionDay(new DateTime(2024, 11, 18), TimeSpan.FromHours(10), TimeSpan.FromHours(14))
            };

            var holidays = new List<Holiday>()
            {
                new Holiday(new DateTime(2024, 11, 19), false)
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingHours,
                holidays);
            exceptionDays.ForEach(workdayCalendar.AddExceptionDay);

            var shipDate = DateTime.Parse("2024-11-20 09:00");
            var timeToProduceProduct = 0.75;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(shipDate, -timeToProduceProduct);

            // Assert
            var expectedDate = DateTime.Parse("2024-11-15 15:00");
            Assert.Equal(expectedDate, result);
        }

        [Fact]
        public void ShippingAtMidnightWithPreceedingHolidayAndExceptionDay()
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
                // 4 hours available
                new ExceptionDay(new DateTime(2024, 11, 11), TimeSpan.FromHours(9), TimeSpan.FromHours(13))
            };

            var holidays = new List<Holiday>()
            {
                new Holiday(new DateTime(2024, 11, 12), false)
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingHours,
                holidays);
            exceptionDays.ForEach(workdayCalendar.AddExceptionDay);

            var wantedTimeToShipProduct = DateTime.Parse("2024-11-13 00:00");
            var workdaysToProduceProduct = 1;

            // Act
            var result = workdayCalendar
                .CalculateLeadTimeWorkdays(wantedTimeToShipProduct, -workdaysToProduceProduct);

            // Assert
            var expectedDate = DateTime.Parse("2024-11-08 12:00");
            Assert.Equal(expectedDate, result);
        }
    }
}
