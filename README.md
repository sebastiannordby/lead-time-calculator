# lead-time-calculator

## How the project is structured

The project is structured in features, where WorkdayCalendarFeature is the "core domain/functionality".
All logic is reserved to the Http API: LeadTimeCalculator.API.

## How i have solved the exercises

I like and think its a good approach to apply test driven development. This ensures that the code remains testable and most likely well structured.

In this exercise i also used a domain driven approach where i started with analyzing the language used in the interview document. When starting out i was not conserned about how the data should be stored, the exercise does not care, and neither does the problem i'm trying to solve.

I started by making a test for the first example BasicWorkingDayCalculationTest, from this test i established the different classes needed to get this test to pass.

## What this project is missing

- Proper persistence (optional)
- Contionous integration (should have been configured when starting out)
- Continous delivery, with IaC (should have configured when starting out)
- Branching strategy (currently push to main)
- Better folder structure, client and api sepaeratly
