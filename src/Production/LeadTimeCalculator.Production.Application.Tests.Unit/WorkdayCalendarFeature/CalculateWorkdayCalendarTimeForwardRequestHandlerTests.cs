using LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculculateTimeForward;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeForward;
using LeadTimeCalculator.Production.Domain.Models.Calendar;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ValidationException = FluentValidation.ValidationException;

namespace LeadTimeCalculator.Production.Application.Tests.Unit.WorkdayCalendarFeature
{
    public class CalculateWorkdayCalendarTimeForwardRequestHandlerTests
    {
        private readonly CalculateWorkdayCalendarTimeForwardRequestHandler _sut;
        private readonly IWorkdayCalendarRepository _workdayCalendarRepositoryMock;

        public CalculateWorkdayCalendarTimeForwardRequestHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddProductionApplicationFeature();

            _workdayCalendarRepositoryMock = Substitute
                .For<IWorkdayCalendarRepository>();
            serviceCollection.AddSingleton(_workdayCalendarRepositoryMock);

            var services = serviceCollection.BuildServiceProvider();
            _sut = services.GetRequiredService<CalculateWorkdayCalendarTimeForwardRequestHandler>();
        }

        [Fact]
        public async Task Errors_on_invalid_request()
        {
            // Given
            var invalidRequest = new CalculateWorkdayCalendarTimeForwardRequest(
                WorkdayCalendarId: -1,
                StartDateTime: DateTime.MinValue,
                WorkdaysToAdd: 0);

            // When & Then
            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _sut.HandleAsync(invalidRequest);
            });

            Assert.Contains(nameof(invalidRequest.WorkdayCalendarId),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.StartDateTime),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.WorkdaysToAdd),
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Calculates_time_forward()
        {
            // Given
            var workdayCalendar = new WorkdayCalendar(
                id: 1,
                defaultWorkdayStartTime: TimeSpan.FromHours(8),
                defaultWorkdayEndTime: TimeSpan.FromHours(16));

            _workdayCalendarRepositoryMock
                .FindAsync(
                    workdayCalendar.Id,
                    Arg.Any<CancellationToken>())
                .Returns(workdayCalendar);

            var validRequest = new CalculateWorkdayCalendarTimeForwardRequest(
                WorkdayCalendarId: 1,
                StartDateTime: DateTime.Parse("2004-12-09 08:00"),
                WorkdaysToAdd: 1);

            // When
            var result = await _sut.HandleAsync(validRequest);

            // Then
            Assert.Equal(validRequest.StartDateTime.AddHours(8), result.EndDateTime);
            Assert.Equal(validRequest.StartDateTime, result.StartDateTime);
            Assert.Equal(validRequest.WorkdaysToAdd, result.WorkdaysAdded);
        }
    }
}
