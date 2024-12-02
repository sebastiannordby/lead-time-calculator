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
        public void CalculateShippingDate_SkipsWeekends()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var receivePartsDayAndStartProduction = DateTime.Parse("2024-11-25 08:00");
            var workDaysToFinishProduct = 5;

            // When
            var minDateTimeAbleToShip = sut
                .CalculateShippingDate(receivePartsDayAndStartProduction, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-12-03 12:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void CalculateShippingDate_SkipsHolidays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

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
                .CalculateShippingDate(startProductionAt, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-11-28 09:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void CalculateShippingDate_HandlesPartialDays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startProductionAt = DateTime.Parse("2024-03-01 09:00");
            var workDaysToFinishProduct = 0.5;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateShippingDate(startProductionAt, workDaysToFinishProduct);

            // Then
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void CalculateShippingDate_HandlesCustomHours()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startProductionAt = DateTime.Parse("2024-10-28 12:00");
            var productionTime = 2;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateShippingDate(startProductionAt, productionTime);

            // Then
            var expectedFinishDate = DateTime.Parse("2024-11-01 12:00"); // Adjusted for valid date
            Assert.Equal(expectedFinishDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void CalculateShippingDate_HandlesExceptionDays()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            workdayCalendar.AddExceptionDay(new(
                DateTime.Parse("2024-11-26"), TimeSpan.FromHours(8), TimeSpan.FromHours(10)));
            workdayCalendar.AddExceptionDay(new(
                DateTime.Parse("2024-11-27"), TimeSpan.FromHours(10), TimeSpan.FromHours(12)));
            workdayCalendar.AddExceptionDay(new(
                DateTime.Parse("2024-11-28"), TimeSpan.FromHours(12), TimeSpan.FromHours(14)));
            workdayCalendar.AddExceptionDay(new(
                DateTime.Parse("2024-11-29"), TimeSpan.FromHours(14), TimeSpan.FromHours(16)));

            var startProductionAt = DateTime.Parse("2024-11-25 08:00");
            var productionTime = 2;

            // When
            var minDateTimeAbleToShip = workdayCalendar
                .CalculateShippingDate(startProductionAt, productionTime);

            // Then
            var expectedFinishDate = DateTime.Parse("2024-11-29 16:00");
            Assert.Equal(expectedFinishDate, minDateTimeAbleToShip);
        }

        [Fact]
        public void CalculateProductionStartDate_SkipsWeekends()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var shippingDeadline = DateTime.Parse("2024-12-03 12:00");
            var productionTime = 5;

            // When
            var productionStartDate = workdayCalendar
                .CalculateProductionStartDateForShipping(shippingDeadline, productionTime);

            // Then
            var expectedStartDate = DateTime.Parse("2024-11-25 08:00");
            Assert.Equal(expectedStartDate, productionStartDate);
        }

        [Fact]
        public void CalculateProductionStartDate_SkipsHolidays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

            var holidays = new List<Holiday>
            {
                new Holiday(DateTime.Parse("2024-11-25"), isRecurring: false),
            };

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var shippingDeadline = DateTime.Parse("2024-11-28 09:00");
            var productionTime = 3;

            // When
            var productionStartDate = workdayCalendar
                .CalculateProductionStartDateForShipping(shippingDeadline, productionTime);

            // Then
            var expectedStartDate = DateTime.Parse("2024-11-22 09:00");
            Assert.Equal(expectedStartDate, productionStartDate);
        }

        [Fact]
        public void CalculateProductionStartDate_HandlesPartialDays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var shippingDeadline = DateTime.Parse("2024-03-01 13:00");
            var productionTime = 0.5;

            // When
            var productionStartDate = workdayCalendar
                .CalculateProductionStartDateForShipping(shippingDeadline, productionTime);

            // Then
            var expectedStartDate = DateTime.Parse("2024-03-01 09:00");
            Assert.Equal(expectedStartDate, productionStartDate);
        }

        [Fact]
        public void CalculateProductionStartDate_HandlesCustomHours()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var shippingDeadline = DateTime.Parse("2024-11-01 12:00");
            var productionTime = 2;

            // When
            var productionStartDate = workdayCalendar
                .CalculateProductionStartDateForShipping(shippingDeadline, productionTime);

            // Then
            var expectedStartDate = DateTime.Parse("2024-10-29 09:00");
            Assert.Equal(expectedStartDate, productionStartDate);
        }

        [Fact]
        public void CalculateProductionStartDate_HandlesExceptionDays()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            workdayCalendar.AddExceptionDay(
                new(DateTime.Parse("2024-11-26"), TimeSpan.FromHours(8), TimeSpan.FromHours(10)));
            workdayCalendar.AddExceptionDay(
                new(DateTime.Parse("2024-11-27"), TimeSpan.FromHours(10), TimeSpan.FromHours(12)));
            workdayCalendar.AddExceptionDay(
                new(DateTime.Parse("2024-11-28"), TimeSpan.FromHours(12), TimeSpan.FromHours(14)));
            workdayCalendar.AddExceptionDay(
                new(DateTime.Parse("2024-11-29"), TimeSpan.FromHours(14), TimeSpan.FromHours(16)));

            var shippingDeadline = DateTime.Parse("2024-11-29 16:00");
            var productionTime = 2;

            // When
            var productionStartDate = workdayCalendar
                .CalculateProductionStartDateForShipping(shippingDeadline, productionTime);

            // Then
            var expectedStartDate = DateTime.Parse("2024-11-25 08:00");
            Assert.Equal(expectedStartDate, productionStartDate);
        }
    }
}
