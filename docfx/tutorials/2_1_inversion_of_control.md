# Inversion of Control and Depency Injection

Onbox relies on inversion of control to provide loosely coupled types. IOC provides guidelines on software architecture should handle the responsability of concrete implementations upper on the hierarchical graph.

There are several ways to implement inversion of control but, by far, the most popular one is IOC Containers, and it is exactly what Onbox uses as method of dealing with this dependencies. These Containers take the responsibility of instantiating and injecting objects by registering their types in a global application scope so they can be consumed anywhere else. This concept is called registration and resolving.

Although not recommended, developers can even ommit the registration phase, as long as the classes consuming these services don't required the abstraction. This process works and you code will compile but it would kill the possibility of mocking these service therefore unit testing them.
