[![Build Status](https://dev.azure.com/onbox/Onbox%20Framework/_apis/build/status/Build%20Docfx?branchName=master)](https://dev.azure.com/onbox/Onbox%20Framework/_build/latest?definitionId=12&branchName=master) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


# Welcome to Onbox Framework

Onbox is a **free and open source** framework to help you build modern cross platform [Revit](https://www.autodesk.com.au/products/revit/overview?plc=RVT&term=1-YEAR&support=ADVANCED&quantity=1) applications in a similar fashion of [Angular](https://angular.io/) and [ASP.Net core](https://dotnet.microsoft.com/apps/aspnet).

The framework and its libraries are designed focusing on modularity, testability and code reuse, it is heavly inspired in modern front end and back end web development, to help you transition from platform to platform without major workflow disruptions.

## Documentation

Checkout the full documentation [here](https://engthiago.github.io/Onboxframework.docs/).

## API Documentation

Checkout the API Docs [here](https://engthiago.github.io/Onboxframework.docs/api/index.html).

## Libraries

As stated above, Onbox is all about modularity, so the framework itself is composed of several libraries that focuses in a specific scope and can be easly extendend or replaced:

| Assembly                           | Short Description                                | Target Framework       |
| -----------------------------------|-------------------------------------------------:|-----------------------:|
| **Onbox.Abstractions**             | Interfaces of all generic usage on the framework | .Net Standard 2.0      |
| **Onbox.Core**                     | Core implementations for all generic services    | .Net Standard 2.0      |
| **Onbox.Di**                       | IOC container system                             | .Net Standard 2.0      |
| **Onbox.Mvc.Abstractions**         | Interfaces for interacting with Views            | .Net Standard 2.0      |
| **Onbox.Mvc**                      | Implementation of WPF MVC Views and Components   | .Net Framework 4.7.2   |
| **Onbox.Mvc.Revit.Abstractions**   | Interfaces for interacting with Revit and Views  | .Net Standard 2.0      |
| **Onbox.Mvc.Revit**                | Implementation of WPF MVC specific to Revit      | .Net Framework 4.7.2   |
| **Onbox.Revit.Abstractions**       | Interfaces for interacting with Revit            | .Net Standard 2.0      |
| **Onbox.Revit**                    | Revit External Applications and Commands         | .Net Framework 4.7.2   |
| **Onbox.Store**                    | State management for Revit MVC / WPF             | .Net Standard 2.0      |
