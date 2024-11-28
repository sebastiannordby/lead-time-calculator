# Lead-Time Calculator

# This branch is based off main

This branch is based off main, but refactored by encapsulating the domain and application logic in their own respective project.
In this case the LeadTimeCalculator.API.Application acts as an ACL(anti corruption layer).

Further refactoring would be to define a context where this WorkdayCalendar belongs, maybe it's a own bounded context?
Given that this is only a simple application, but could envolve to something bigger i will set the line for refactoring to here.
This is overenginnering at it finest already and the initial structuring is appropriate for this size of a project.
Given the size of the project the developer(s) should be able to follow principles set in their coding standard and adhere to them.

Mediator could have been used to dispatch requests to the application layer, to avoid having to reference handlers directly in the API-project.

By restructuring the project like this we can

- Unit test domain logic without mocking/fakes
- Unit test application logic with mocking/fakes
- Integration test as usual the API to get the whole workflow
- Allow for architectural testing

## How to Run the Project

To run this project, you will need the following prerequisites:

1. **Visual Studio 2022** (Community, Professional, or Enterprise):

   - You can download it from [here](https://visualstudio.microsoft.com/downloads/).

2. **.NET 9 SDK**:

   - This project is built on .NET 9, so you will need to install the SDK from [Microsoft's official website](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

3. **Aspire**:
   - Instead of using Docker Compose, I've chosen **Aspire**, as it’s native to .NET. To install Aspire, the easiest way is through the Visual Studio Installer. Detailed instructions are available on [Microsoft's documentation](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio).

Once you have the necessary tools installed:

- Clone this repository or download it as a ZIP file.
- Open the solution file in Visual Studio.
- Set **"LeadTimeCalculator.Orchestrator"** as the startup project by right-clicking the project in the Solution Explorer and selecting **Set as StartUp Project**.
- Finally, click the green **Run** button or press **F5** to launch the application.

## Project Structure

The project is organized into features, with the **WorkdayCalendarFeature** as the core domain and functionality. All business logic is encapsulated in the **LeadTimeCalculator.API** HTTP API.

## Approach to Solving the Exercises

I prefer and advocate for **Test-Driven Development (TDD)**, as it ensures that the code remains clean, testable, and well-structured.

For this exercise, I applied a **Model-Driven Design with some **Domain-Driven Design (DDD)**, beginning by analyzing the terminology used in the interview document and create the models needed for the required functionality. Initially, I was not concerned with how the data should be stored, as the problem doesn't dictate a specific persistence strategy.

My approach began by creating a test for the first example, **BasicWorkingDayCalculationTest**. This test served as the foundation for determining the necessary classes and logic to make the test pass.

## What’s Missing in This Project

There are several areas for improvement that I would address in future iterations of this project:

- **Logging**: Currently not implemented.
- **Proper Persistence**: While optional, this could be improved.
- **Continuous Integration**: This should have been configured from the start.
- **Continuous Delivery with IaC**: This should also be configured from the beginning.
- **Branching Strategy**: Currently, everything is pushed to the `main` branch.
- **Folder Structure**: The project could benefit from separating the client and API into distinct folders.
- **Security/Quality Code Scanning**: Tools like **Snyk** and/or **SonarCloud** should be integrated for scanning and monitoring security and quality.
- **Frontend Tests**: There are no tests for the frontend at the moment.
- **Frontend Formatting Rules**: Although not critical, some formatting rules (e.g., using **Prettier**) could be applied, especially for Razor pages.

## General Notes

- I have not in any way, shape, or form gone out of my way to optimize any of the algorithms made/used. My goal was to solve the actual problem at hand (the calendar) with a testable, domain-driven approach.
- The frontend could use some additional work, but given the short timeframe and my busy schedule, I did my best to implement a functional solution within the constraints.
- I have not used AI for anything other than polishing this README file. The code itself is written solely by me, **Sebastian Nordby**.

Check out the YouTube video for how to use the software: https://www.youtube.com/watch?v=9zVJeKKJZnA
