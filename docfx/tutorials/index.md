# Welcome to Onbox Framework

Onbox is a **free and open source** framework to help you build modern cross platform [Revit](https://www.autodesk.com.au/products/revit/overview?plc=RVT&term=1-YEAR&support=ADVANCED&quantity=1) applications in a similar fashion of [Angular](https://angular.io/) and [ASP.Net core](https://dotnet.microsoft.com/apps/aspnet).

The framework and its libraries are designed focusing on modularity, testability and code reuse, it is heavly inspired in modern front end and back end web development, to help you transition from platform to platform without major workflow disruptions.

## Compatibility

Revit 2019, .NetStandard 2.0, and .Net Framework 4.7.2 or later.

## Getting Started

Click [here](./guetstart.md) to go to the getting started guide.


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

## Dependencies

The philosophy behind Onbox is to keep dependencies to minimum, so the only runtime dependency aside from Revit and .Net is [Newtonsoft.Json](https://www.newtonsoft.com/json), which ships with Revit anyway.

The MVC libraries also require [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged) to automatically call [PropertyChanged Event](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netframework-4.7.2) to update the UI when a binding changes. This dependency is only required at compile time though.

## UI

For the UI design pattern, Onbox ships with **WPF MVC** libraries, this choice was made because of the powerful and ease to use WPF's [Data Binding System](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/data-binding-overview?view=netdesktop-5.0) and the popularity of MVC in modern web frameworks. This approach modernizes Revit API programming and makes it easier for developers to switch from/to [Angular](https://angular.io/) / [ASP.Net core](https://dotnet.microsoft.com/apps/aspnet) / [Laravel](https://laravel.com/) / [Spring](https://spring.io/) when building apps that communicate with the cloud.

With that being said, if you are using any other type of UI design pattern or layering e.g. WPF MVVM or even Windows Forms and like to keep it that way, you can absolutely do so by handling the UI interactions yourself, and consuming `Onbox.Revit`, `Onbox.Core`, and `Onbox.Di` with no reference to any Onbox's MVC libraries.

## Testing

Because of the need of having a valid Revit Context to interact with specific objects like Applications, Documents, and Elements, creating automated tests for Revit was always a challange. Onbox ships with a dependency injection system, allowing developers to abstract away interfaces from their implementations, injecting then when and where they are needed on specific scopes, therefore making it possible to mock and test these classes independently.

 You could the automate your tests by using Dynamo's [RevitTestFramework](https://github.com/DynamoDS/RevitTestFramework) or Geberit's [Revit.TestRunner](https://github.com/geberit/Revit.TestRunner).

## Versioning

All libraries within Onbox have versioning namespaces to avoid [Revit dll conficts](https://thebuildingcoder.typepad.com/blog/2017/06/handling-third-party-library-dll-conflicts.html). While this aproach solves the issue of [dll hell](https://archi-lab.net/dll-hell-is-real/) and enables different versions of the libraries to co-exist within the same Revit session, it also means that the framework needs to be more conservative on its versioning sytem. The namespaces will jump from integer numbers instead of semantic versioning.

Updating from one version to another will require you to update your using directives too, as an example, changing from version 7 to version 8 would be:

``` C#
using Onbox.Abstractions.V7;
using Onbox.Revit.V7.Commands;
```

*to..*


``` C#
using Onbox.Abstractions.V8;
using Onbox.Revit.V8.Commands;
```

## How to Contribute

Since the framework is open source, feel free to fork the code, create log issues, report bugs, and make pull-requests. Documentation and step by step tutorials is where it mostly need contributions at this point, so if you are a specialist, please join in!!! 😃
