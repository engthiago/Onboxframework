# Separation of Concerns and Single Responsability Principle

In software engineering, separation of concerns is the concept of separating small pieces of a system larger system, each one responsible and specialized for doing one single thing, and have only a single reason to change.

Often times we create the so called [God Classes](https://en.wikipedia.org/wiki/God_object). They tend to grow larger and larger on each iteration of our software, becoming, in each iteration, harder and harder to maintain. Having classes like this can hurt the development and the quality of the system.

To remedy that problem we can refactor that code so we devide that big class in several smaller and easy to maintain classes, each responsible for a specific task. That way, if a bug emerges or if we need to change how that class works we can easily do so, it will be much more obivious where to look and change.

Onbox tries to avoid God Classes and Methods as much as possible on its implementations and APIs. One example is on the [Application](1_1_application.md) entry point where the famework have two separate methods on the startup lifecycle, one responsible for dealing with the UI (Ribbon) and other to startup the actual business logic for the application. Also, the implementation of the [IOC Container](3_ioc_container.md) helps developers to refactor their code and handle the responsibility of instation to the container.