# DVTElevatorChallenge

Welcome to the **DVTElevatorChallenge** repository! This project simulates an elevator control system, serving as a practical demonstration of software engineering principles and practices.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Solution Structure](#solution-structure)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Dependency Injection](#dependency-injection)
- [Asynchronous Programming](#asynchronous-programming)
- [Architecture and Design](#architecture-and-design)
- [Contributing](#contributing)
  
---

## Overview

**DVTElevatorChallenge** is a .NET 9 application designed to simulate and manage elevator operations. It includes features such as moving elevators to specific floors, managing passengers, and generating a list of elevators with randomized states.

---

## Features

- Simulate elevator movements between floors.
- Add passengers to elevators.
- Generate a list of elevators with random current floors.
- Move elevators to user-specified floors and destination floors.

---

## Solution Structure

The solution is organized into the following projects:

- **DVTElevatorChallengeTest**: The main solution container.
- **DVTElevatorChallengeTest.Application**: Contains application logic and service interfaces.
- **DVTElevatorChallengeTest.ConsoleApp**: The console-based UI for interacting with the application.
- **DVTElevatorChallengeTest.Core**: Includes core domain models and business logic.
- **DVTElevatorChallengeTest.Infrastructure**: Implements data access and external dependencies.
- **DVTElevatorChallengeTest.UnitTests**: Contains unit tests for ensuring code quality and reliability.

---

## Requirements

- [.NET 9](https://dotnet.microsoft.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

---

## Installation

To install and run the **DVTElevatorChallenge** project, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/NyikoNgobeni/DVTElevatorChallenge.git
   ```

2. Navigate to the project directory:
   ```bash
   cd DVTElevatorChallenge
   ```

3. Build the project using Visual Studio 2022 or your preferred IDE.

---

## Usage

1. Run the application.
2. Use the user interface to simulate elevator requests.
3. Observe the elevator's response to requests and its movement between floors.

---

## Dependency Injection

The project makes extensive use of **Dependency Injection (DI)** to achieve loose coupling and promote testability. Below is an overview of how DI is implemented:

1. **Service Registration**: All services and repositories are registered in the DI container in the `Startup.cs` or `Program.cs` file:

2. **Injecting Dependencies**: Services and repositories are injected wherever required using constructor injection. For example:
   ```csharp
   public class ElevatorController
   {
       private readonly IElevatorService _elevatorService;

       public ElevatorController(IElevatorService elevatorService)
       {
           _elevatorService = elevatorService;
       }

       public void MoveElevator(int targetFloor)
       {
           _elevatorService.MoveToFloor(targetFloor);
       }
   }
   ```

3. **Mocking for Unit Tests**: DI makes it easy to mock dependencies during unit testing:
   ```csharp
   var mockElevatorService = new Mock<IElevatorService>();
   mockElevatorService.Setup(x => x.MoveToFloor(It.IsAny<int>())).Returns(true);

   var controller = new ElevatorController(mockElevatorService.Object);
   ```

---

## Asynchronous Programming

The **DVTElevatorChallenge** project utilizes asynchronous programming to enhance performance and ensure a responsive user experience. Key points:

1. **Async Methods**: All time-consuming operations, such as moving elevators or managing passengers, are implemented asynchronously.
   ```csharp
   public async Task MoveToFloorAsync(int floor)
   {
       await Task.Delay(1000); // Simulate the elevator movement delay
       Console.WriteLine($"Elevator moved to floor {floor}");
   }
   ```

2. **Error Handling**: All asynchronous methods include robust exception handling to manage errors effectively.
   ```csharp
   try
   {
       await _elevatorService.MoveToFloorAsync(floor);
   }
   catch (Exception ex)
   {
       Console.WriteLine($"Error: {ex.Message}");
   }
   ```

Using asynchronous programming ensures that the application remains performant and scalable, especially when handling multiple elevator requests simultaneously.

---

## Architecture and Design

The **DVT Elevator Challenge** project incorporates key software engineering principles and patterns:

- **Clean Architecture**: Separation of concerns ensures that core logic remains independent of external frameworks.
- **SOLID Principles**: Adherence to SOLID principles provides a maintainable and scalable codebase.
- **Test-Driven Development (TDD)**: Ensures that all functionalities are covered by unit tests.
- **Repository Pattern**: Manages data access, maintaining separation between the data layer and business logic.
- **Singleton Pattern**: Ensures that certain components have a single instance throughout the application's lifecycle.
- **Error Handling**: Comprehensive exception management for smooth application performance.

---

## Contributing

We welcome contributions to the **DVT Elevator Challenge** project! To contribute:

1. Fork the repository.
2. Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add feature"
   ```
4. Push to the branch:
   ```bash
   git push origin feature-name
   ```
5. Open a pull request.

---



Feel free to customize this README to better fit your project's specifics. If you have any suggestions or need further assistance, let me know!
