using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature;
using LeadTimeCalculator.API.Domain.WorkdayCalendarFeature.Models;

namespace LeadTimeCalculator.API.Tests.Unit.Features.WorkdayCalendarFeature
{
    public class WorkdayCalendarTests
    {
        [Fact]
        public void AddWorkingDays_SkipsWeekends()
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

            var startDate = DateTime.Parse("2024-11-25 08:00");
            var workdaysToAdd = 5;

            // When
            var resultDate = sut
                .AddWorkingDays(startDate, workdaysToAdd);

            // Then
            var expectedDate = DateTime.Parse("2024-12-03 12:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void AddWorkingDays_SkipsHolidays()
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

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var startDate = DateTime.Parse("2024-11-22 09:00");
            var workdaysToAdd = 3;

            // When
            var resultDate = sut
                .AddWorkingDays(startDate, workdaysToAdd);

            // Then
            var expectedDate = DateTime.Parse("2024-11-28 09:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void AddWorkingDays_HandlesPartialDays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startDate = DateTime.Parse("2024-03-01 09:00");
            var workdaysToAdd = 0.5;

            // When
            var resultDate = sut
                .AddWorkingDays(startDate, workdaysToAdd);

            // Then
            var expectedDate = DateTime.Parse("2024-03-01 13:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void AddWorkingDays_HandlesCustomHours()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startDate = DateTime.Parse("2024-10-28 12:00");
            var workdaysToAdd = 2;

            // When
            var resultDate = sut
                .AddWorkingDays(startDate, workdaysToAdd);

            // Then
            var expectedDate = DateTime.Parse("2024-11-01 12:00"); // Adjusted for valid date
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void AddWorkingDays_HandlesExceptionDays()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            sut.AddExceptionDay(new(
                DateTime.Parse("2024-11-26"), TimeSpan.FromHours(8), TimeSpan.FromHours(10)));
            sut.AddExceptionDay(new(
                DateTime.Parse("2024-11-27"), TimeSpan.FromHours(10), TimeSpan.FromHours(12)));
            sut.AddExceptionDay(new(
                DateTime.Parse("2024-11-28"), TimeSpan.FromHours(12), TimeSpan.FromHours(14)));
            sut.AddExceptionDay(new(
                DateTime.Parse("2024-11-29"), TimeSpan.FromHours(14), TimeSpan.FromHours(16)));

            var startDate = DateTime.Parse("2024-11-25 08:00");
            var workdaysToAdd = 2;

            // When
            var resultDate = sut
                .AddWorkingDays(startDate, workdaysToAdd);

            // Then
            var expectedDate = DateTime.Parse("2024-11-29 16:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void SubtractWorkingDays_SkipsWeekends()
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

            var startDate = DateTime.Parse("2024-12-03 12:00");
            var workdaysToSubtract = 5;

            // When
            var resultDate = sut
                .SubtractWorkingDays(startDate, workdaysToSubtract);

            // Then
            var expectedDate = DateTime.Parse("2024-11-25 08:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void SubtractWorkingDays_SkipsHolidays()
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

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                holidays);

            var startDate = DateTime.Parse("2024-11-28 09:00");
            var workdaysToSubtract = 3;

            // When
            var resultDate = sut
                .SubtractWorkingDays(startDate, workdaysToSubtract);

            // Then
            var expectedDate = DateTime.Parse("2024-11-22 09:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void SubtractWorkingDays_HandlesPartialDays()
        {
            // Given
            var sameWorkingHoursForWholeWeek = new WorkHours(
                TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var defaultWorkingDays = new WorkWeek(
                sameWorkingHoursForWholeWeek);

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startDate = DateTime.Parse("2024-03-01 13:00");
            var workdaysToSubtract = 0.5;

            // When
            var resultDate = sut
                .SubtractWorkingDays(startDate, workdaysToSubtract);

            // Then
            var expectedDate = DateTime.Parse("2024-03-01 09:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void SubtractWorkingDays_HandlesCustomHours()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                mondayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                tuesdayWorkingHours: new WorkHours(TimeSpan.FromHours(9), TimeSpan.FromHours(13)),
                wednesdayWorkingHours: new WorkHours(TimeSpan.FromHours(10), TimeSpan.FromHours(14)),
                thursdayWorkingHours: new WorkHours(TimeSpan.FromHours(11), TimeSpan.FromHours(15)),
                fridayWorkingHours: new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            var startDate = DateTime.Parse("2024-11-01 12:00");
            var workdaysToSubtract = 2;

            // When
            var resultDate = sut
                .SubtractWorkingDays(startDate, workdaysToSubtract);

            // Then
            var expectedDate = DateTime.Parse("2024-10-29 09:00");
            Assert.Equal(expectedDate, resultDate);
        }

        [Fact]
        public void SubtractWorkingDays_HandlesExceptionDays()
        {
            // Given
            var defaultWorkingDays = new WorkWeek(
                new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16)));

            var sut = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                defaultWorkingDays,
                Enumerable.Empty<Holiday>());

            sut.AddExceptionDay(
                new(DateTime.Parse("2024-11-26"), TimeSpan.FromHours(8), TimeSpan.FromHours(10)));
            sut.AddExceptionDay(
                new(DateTime.Parse("2024-11-27"), TimeSpan.FromHours(10), TimeSpan.FromHours(12)));
            sut.AddExceptionDay(
                new(DateTime.Parse("2024-11-28"), TimeSpan.FromHours(12), TimeSpan.FromHours(14)));
            sut.AddExceptionDay(
                new(DateTime.Parse("2024-11-29"), TimeSpan.FromHours(14), TimeSpan.FromHours(16)));

            var startDate = DateTime.Parse("2024-11-29 16:00");
            var workdaysToSubtract = 2;

            // When
            var resultDate = sut
                .SubtractWorkingDays(startDate, workdaysToSubtract);

            // Then
            var expectedDate = DateTime.Parse("2024-11-25 08:00");
            Assert.Equal(expectedDate, resultDate);
        }
    }
}
