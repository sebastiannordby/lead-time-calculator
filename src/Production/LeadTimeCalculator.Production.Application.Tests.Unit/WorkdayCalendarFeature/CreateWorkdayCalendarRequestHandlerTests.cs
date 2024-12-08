using Bogus;
using LeadTimeCalculator.Production.Application.Calendar;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.Contracts;
using LeadTimeCalculator.Production.Application.Calendar.UseCases.CreateCalendar;
using LeadTimeCalculator.Production.Contracts.Calendar.CreateCalendar;
using LeadTimeCalculator.Production.Domain.Models.Calendar;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ValidationException = FluentValidation.ValidationException;

namespace LeadTimeCalculator.Production.Application.Tests.Unit.WorkdayCalendarFeature
{
    public class CreateWorkdayCalendarRequestHandlerTests
    {
        private readonly Faker _faker = new();
        private readonly CreateWorkdayCalendarRequestHandler _sut;
        private readonly IWorkdayCalendarRepository _workdayCalendarRepositoryMock;

        public CreateWorkdayCalendarRequestHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWorkdayCalendarApplicationFeature();

            _workdayCalendarRepositoryMock = Substitute.For<IWorkdayCalendarRepository>();
            serviceCollection.AddSingleton(_workdayCalendarRepositoryMock);

            var services = serviceCollection.BuildServiceProvider();
            _sut = services.GetRequiredService<CreateWorkdayCalendarRequestHandler>();
        }

        [Fact]
        public async Task GivenInvalidRequest_ShouldErrorWithInvalidFields()
        {
            // Given
            var invalidRequest = new CreateWorkdayCalendarRequest(
                DefaultWorkdayStartTime: TimeSpan.MinValue,
                DefaultWorkdayEndTime: TimeSpan.MinValue);

            // When & Then
            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _sut.HandleAsync(invalidRequest);
            });

            Assert.Contains(nameof(invalidRequest.DefaultWorkdayEndTime),
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GivenValidRequest_ShouldPersistCalendar()
        {
            // Given
            var validRequest = new CreateWorkdayCalendarRequest(
                DefaultWorkdayStartTime: TimeSpan.FromHours(8),
                DefaultWorkdayEndTime: TimeSpan.FromHours(16));

            var calendarId = _faker.Random.Int(1);
            _workdayCalendarRepositoryMock
                .GetNewCalendarNumberAsync(default)
                .ReturnsForAnyArgs(calendarId);

            // When
            await _sut.HandleAsync(validRequest);

            // Then
            await _workdayCalendarRepositoryMock
                .Received(1)
                .SaveAsync(
                    Arg.Is<WorkdayCalendar>(x =>
                        x.Id == calendarId
                        && x.WorkWeek.MondayWorkingHours.StartTime == validRequest.DefaultWorkdayStartTime
                        && x.WorkWeek.MondayWorkingHours.EndTime == validRequest.DefaultWorkdayEndTime),
                    Arg.Any<CancellationToken>());
        }
    }
}
