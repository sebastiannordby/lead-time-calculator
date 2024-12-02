using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;

namespace LeadTimeCalculator.API.Tests.Unit.Features.WorkdayCalendarFeature
{
    /// <summary>
    /// Unit tests for calculating production and shipping dates with workday calendars.
    /// Covers scenarios with holidays, weekends, and exception days.
    /// </summary>
    public class WorkdayCalendarTests
    {
        [Fact]
        public void GivenFiveWorkdaysAndNoHolidays_WhenCalculateWhenCanShipWhenProductionStartsAt_ThenSkipsWeekends()
        {
            // Given
            var defaultWorkingDays = new WorkWeek()
            {
                MondayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                TuesdayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                WednesdayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                ThursdayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                FridayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-11-25 08:00");
            var workDaysToFinishProduct = 5;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateWhenCanShipWhenProductionStartsAt(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-12-03 12:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void GivenAHolidayInTheMiddleOfProduction_WhenCalculateWhenCanShipWhenProductionStartsAt_ThenSkipsHoliday()
        {
            // Given
            var eightToFour = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek()
            {
                MondayWorkingHours = eightToFour,
                TuesdayWorkingHours = eightToFour,
                WednesdayWorkingHours = eightToFour,
                ThursdayWorkingHours = eightToFour,
                FridayWorkingHours = eightToFour
            };

            var holidays = new List<Holiday>
            {
                new Holiday(DateTime.Parse("2024-11-25"), isRecurring: false),
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var startProductionAt = DateTime.Parse("2024-11-22 09:00");
            var workDaysToFinishProduct = 3;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateWhenCanShipWhenProductionStartsAt(startProductionAt, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-11-28 09:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void GivenFractionalWorkdays_WhenCalculateWhenCanShipWhenProductionStartsAt_ThenRespectsDailyWorkingHours()
        {
            // Given
            var eightToFour = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek()
            {
                MondayWorkingHours = eightToFour,
                TuesdayWorkingHours = eightToFour,
                WednesdayWorkingHours = eightToFour,
                ThursdayWorkingHours = eightToFour,
                FridayWorkingHours = eightToFour
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startProductionAt = DateTime.Parse("2024-03-01 09:00");
            var workDaysToFinishProduct = 0.5;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateWhenCanShipWhenProductionStartsAt(startProductionAt, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void GivenFractionalWorkdays_WhenCalculateWhenCanShipWhenProductionStartsAt_ThenRespectsDailyWorkingHours_2()
        {
            // Given
            var workingHours = new WorkWeek
            {
                MondayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)), // 4 hours
                TuesdayWorkingHours = new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)), // 4 hours
                WednesdayWorkingHours = new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)), // 4 hours
                ThursdayWorkingHours = new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)), // 4 hours
                FridayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                workingHours,
                Enumerable.Empty<Holiday>());

            var startProductionAt = DateTime.Parse("2024-10-28 12:00");
            var productionTime = 2;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateWhenCanShipWhenProductionStartsAt(startProductionAt, productionTime);

            // Then
            var expectedFinishDate = DateTime.Parse("2024-11-01 12:00"); // Adjusted for valid date
            Assert.Equal(expectedFinishDate, minDateTimeAbleToShip);
        }


        [Fact]
        public void GivenFractionalWorkdays_WhenCalculateProductionTimeWhenHaveToShipA_ThenExceedsDailyWorkingHours()
        {
            // Given
            var workingHours = new WorkWeek
            {
                MondayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)), // 4 hours
                TuesdayWorkingHours = new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)), // 4 hours
                WednesdayWorkingHours = new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)), // 4 hours
                ThursdayWorkingHours = new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)), // 4 hours
                FridayWorkingHours = new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)), // 8 hours
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                workingHours,
                Enumerable.Empty<Holiday>());

            var wantingToShipAt = DateTime.Parse("2024-11-29 12:00");
            var productionTime = 2.5;

            // When
            var minRequiredStartProductionTime = workdayCalendar
                .CalculateProductionTimeWhenHaveToShipAt(wantingToShipAt, productionTime);

            // Then
            var expectedFinishDate = DateTime.Parse("2024-11-25 08:00");
            Assert.Equal(expectedFinishDate, minRequiredStartProductionTime);
        }

    }
}
