# lead-time-calculator

## How do i run this project?

Preferably you have Visual Studio 2022 installed, community, professional or enterprise, does not matter.

This project is built on .NET 9, so you need to install the SDK from Microsoft:
https://dotnet.microsoft.com/en-us/download/dotnet/9.0

Instead of using Docker Compose for orchestrating the application, i choose Aspire, because its .NET native.
You need to have it installed, the easiest way it through Visual Studio Installer, explained by Microsoft:
https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio

Either clone the repository or download it as a ZIP file. Open the solution file with Visual Studio and set
"LeadTimeCalculator.Orchestrator" as startup project, by right clicking the project in the solution explorer inside Visual Studio. Now click the green run button on top or "F5" on your keyboard.

## How the project is structured

The project is structured in features, where WorkdayCalendarFeature is the "core domain/functionality".
All logic is reserved to the Http API: LeadTimeCalculator.API.

## How i have solved the exercises

I like and think its a good approach to apply test driven development. This ensures that the code remains testable and most likely well structured.

In this exercise i also used a domain driven approach where i started with analyzing the language used in the interview document. When starting out i was not conserned about how the data should be stored, the exercise does not care, and neither does the problem i'm trying to solve.

I started by making a test for the first example BasicWorkingDayCalculationTest, from this test i established the different classes needed to get this test to pass.

## What this project is missing

- Proper persistence (optional)
- Continuous integration (should have been configured when starting out)
- Continuous delivery, with IaC (should have configured when starting out)
- Branching strategy (currently push to main)
- Better folder structure, client and api sepaeratly
- Security/Quality code scanning, Snyk and/or SonarCloud
