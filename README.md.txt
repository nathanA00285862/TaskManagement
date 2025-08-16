# Task Management System

##Overview
This is a console-based Task Management System built using .NET 8.0, following Clean Architecture principles. The application allows users to create, view, update, and delete tasks, with an optional "Task Prioritization" feature controlled by a feature flag. It also supports exporting tasks to various formats (e.g., JSON) via a dynamic plugin system.

## Architecture
The solution is structured into the following projects, adhering to Clean Architecture:
- **TaskManagement.Domain**: Contains business entities (`TaskItem`) and repository interfaces (`ITaskRepository`). No dependencies.
- **TaskManagement.Application**: Implements use cases using CQRS with MediatR. Depends on `TaskManagement.Domain`.
- **TaskManagement.Infrastructure**: Provides a concrete in-memory implementation of `ITaskRepository`. Depends on `TaskManagement.Domain`.
- **TaskManagement.Presentation**: Console app for user interaction, dispatching commands/queries via MediatR. Depends on `TaskManagement.Application` and `TaskManagement.Infrastructure`.
- **Exporter.Abstractions**: Defines the `IExporter` interface for plugins. No dependencies.
- **JsonExporterPlugin**: A sample plugin for JSON export. Depends on `Exporter.Abstractions` and `TaskManagement.Domain`.
- **TaskManagement.Tests**: Contains unit tests for Application layer handlers using xUnit and Moq.

## Building and Running
1. **Prerequisites**: Visual Studio 2022 with .NET 8.0 SDK (or .NET 8.0 CLI).
2. **Open the Solution**:
   - Load `TaskManagement.sln` in Visual Studio.
3. **Build the Solution**:
   - Go to **Build > Build Solution** (or press `Ctrl+Shift+B`).
4. **Run the Application**:
   - Set `TaskManagement.Presentation` as the startup project (right-click > **Set as Startup Project**).
   - Press `F5` or go to **Debug > Start Debugging**.
5. **Interact**: Use the menu-driven interface to manage tasks (options 1-7).

## Feature Flags
The `TaskPrioritization` feature is controlled via `appsettings.json` in the `TaskManagement.Presentation` project:
```json
{
  "FeatureManagement": {
    "TaskPrioritization": true
  }
}