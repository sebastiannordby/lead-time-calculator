using LeadTimeCalculator.Production.Application.Calendar.UseCases.CalculateTimeBackward;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Contracts.Calendar.CalculateTimeBackward;
using LeadTimeCalculator.Production.Domain.Models.Calendar;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ValidationException = FluentValidation.ValidationException;

namespace LeadTimeCalculator.Production.Application.Tests.Unit.WorkdayCalendarFeature
{
    public class CalculateWorkdayCalendarTimeBackwardRequestHandlerTests
    {
        private readonly CalculateWorkdayCalendarTimeBackwardRequestHandler _sut;
        private readonly IWorkdayCalendarRepository _workdayCalendarRepositoryMock;

        public CalculateWorkdayCalendarTimeBackwardRequestHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddProductionApplicationFeature();

            _workdayCalendarRepositoryMock = Substitute
                .For<IWorkdayCalendarRepository>();
            serviceCollection.AddSingleton(_workdayCalendarRepositoryMock);

            var services = serviceCollection.BuildServiceProvider();
            _sut = services.GetRequiredService<CalculateWorkdayCalendarTimeBackwardRequestHandler>();
        }

        [Fact]
        public async Task Errors_on_invalid_request()
        {
            // Given
            var invalidRequest = new CalculateWorkdayCalendarTimeBackwardRequest(
                WorkdayCalendarId: -1,
                DateTimeToSubtractFrom: DateTime.MinValue,
                WorkdaysToSubtract: 0);

            // When & Then
            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _sut.HandleAsync(invalidRequest);
            });

            Assert.Contains(nameof(invalidRequest.WorkdayCalendarId),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.DateTimeToSubtractFrom),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.WorkdaysToSubtract),
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Calculates_time_backward()
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

            var validRequest = new CalculateWorkdayCalendarTimeBackwardRequest(
                WorkdayCalendarId: 1,
                DateTimeToSubtractFrom: DateTime.Parse("2004-12-13 16:00"),
                WorkdaysToSubtract: 1);

            // When
            var result = await _sut.HandleAsync(validRequest);

            // Then
            Assert.Equal(validRequest.DateTimeToSubtractFrom.AddHours(-8), result.EndDateTime);
            Assert.Equal(validRequest.WorkdaysToSubtract, result.WorkdaysSubtracted);
            Assert.Equal(validRequest.DateTimeToSubtractFrom, result.StartDateTime);
        }
    }
}
