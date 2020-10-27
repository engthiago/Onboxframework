| Project | Status |
| ------- | ------ |
| License | [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) |
| Build Nuget Packages | [![Build Status](https://dev.azure.com/onbox/Onbox%20Framework/_apis/build/status/Build%20Docfx?branchName=master)](https://dev.azure.com/onbox/Onbox%20Framework/_build/latest?definitionId=12&branchName=master) |
| Unit Tests | [![Build Status](https://dev.azure.com/onbox/Onbox%20Framework/_apis/build/status/Unit%20Tests?branchName=master)](https://dev.azure.com/onbox/Onbox%20Framework/_build/latest?definitionId=13&branchName=master) |

# Welcome to Onbox Framework

Onbox is a **free and open source** framework to help you build modern cross platform [Revit](https://www.autodesk.com.au/products/revit/overview?plc=RVT&term=1-YEAR&support=ADVANCED&quantity=1) applications in a similar fashion of [Angular](https://angular.io/) and [ASP.Net core](https://dotnet.microsoft.com/apps/aspnet).

The framework and its libraries are designed focusing on modularity, testability and code reuse, it is heavly inspired in modern front end and back end web development, to help you transition from platform to platform without major workflow disruptions.

## Documentation

Documentation is in its early stages, (yeah I know.. documentation is important).
Checkout the documentation [here](https://engthiago.github.io/Onboxframework.docs/).

## API Documentation

Checkout the API Docs [here](https://engthiago.github.io/Onboxframework.docs/api/index.html).

## Libraries

As stated above, Onbox is all about modularity, so the framework itself is composed of several libraries that focuses in a specific scope and can be easly extendend or replaced:

| Assembly                           | Short Description                                | Target Framework       |
| -----------------------------------|-------------------------------------------------:|-----------------------:|
| [Onbox.Abstractions](https://www.nuget.org/packages/Onbox.Abstractions/)             | Interfaces of all generic usage on the framework | .Net Standard 2.0      |
| [Onbox.Core](https://www.nuget.org/packages/Onbox.Core/)                     | Core implementations for all generic services    | .Net Standard 2.0      |
| [Onbox.Di](https://www.nuget.org/packages/Onbox.Core/)                       | IOC container system                             | .Net Standard 2.0      |
| [Onbox.Mvc.Abstractions](https://www.nuget.org/packages/Onbox.Abstractions/)        | Interfaces for interacting with Views            | .Net Standard 2.0      |
| [Onbox.Mvc](https://www.nuget.org/packages/Onbox.Mvc/)                      | Implementation of WPF MVC Views and Components   | .Net Framework 4.7.2   |
| [Onbox.Mvc.Revit.Abstractions](https://www.nuget.org/packages/Onbox.Mvc.Revit.Abstractions/)   | Interfaces for interacting with Revit and Views  | .Net Standard 2.0      |
| [Onbox.Mvc.Revit](https://www.nuget.org/packages/Onbox.Mvc.Revit/)                | Implementation of WPF MVC specific to Revit      | .Net Framework 4.7.2   |
| [Onbox.Revit.Abstractions](https://www.nuget.org/packages/Onbox.Revit.Abstractions/)       | Interfaces for interacting with Revit            | .Net Standard 2.0      |
| [Onbox.Revit](https://www.nuget.org/packages/Onbox.Revit/)                    | Revit External Applications and Commands         | .Net Framework 4.7.2   |
| [Onbox.Store](https://www.nuget.org/packages/Onbox.Store/)                    | State management for Revit MVC / WPF             | .Net Standard 2.0      |


We have built parts of several applications using some of these libraries, our current working project, [Shedmate app](https://construction.autodesk.com/integrations/shedmate) is the one that relies on it the most. I would say parts of it because only some libraries existed when we started coding the app. Shedmate is a web-based 3d configurator that needs to process the same data models in different places: Revit, our ASPNet Cloud server, and our front end Angular app. The cool thing here is that we can share services between the Revit and ASP and even run a script to generate our data models in typescript for Angular!

## Simplicity

None of the features that Onbox provides out of the box are really trying to be the best ones in the industry. The framework is made with simplicity in mind, we tried to simplify the implementations as much as we could, even when we wanted to provide more functionality, we tried to make it easy to consume simpler versions of the APIs. 

## Modularity

The framework aims for modularity, so the idea here is that you can introduce new functionality by yourself. Our libraries, e.g. Container, Mapper, State Management, Async are tiny and are not trying to solve every single problem or implement every single feature, also they can always be replaced by more mature ones out there. 

## Flexible

The framework also aims to be flexible, if you have an existing Revit plugin and want to give Onbox a try, you would just swap the implementation for your ExternalApplication and then for the ExternalCommand(s) you want the container to be injected on. You are good to go!

## Testability

With the loosely coupled architecture that the framework helps you to build, you can then use any testing frameworks like [Dynamo's Revit Tester Framework](https://github.com/DynamoDS/RevitTestFramework) or [Geberit's Revit Test Runner](https://github.com/geberit/Revit.TestRunner). We are even using [Design Automation on Forge](https://forge.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/?_ga=2.215688401.7327333.1603131319-920645407.1589401464) to unit test our Revit Apps, that way, everything can be integrated into a CI/CD pipeline.

## Usage and Collaboration

Everyone is welcome to use it and collaborate! Being MIT licensed, you can take any parts of the framework and modify it to your usage, this is simple, because the libraries are tiny. The same thing for eventual bugs, you can step in and fix them yourself or log an issue on Github. We would appreciate any code pull requests, contributions to the documentation, and publications you make for it. The idea is to have a mature Framework so everyone can collect the benefits.
