using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Di.VDev
{
    /// <summary>
    /// Onbox's IOC container implementation
    /// </summary>d
    public class Container : IContainer, IDisposable
    {
        /// <summary>
        /// The list of current transient types currently registered
        /// </summary>
        protected IDictionary<Type, Type> transientTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// The list of current scoped types currently registered
        /// </summary>
        protected IDictionary<Type, Type> scopedTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// The list of current singleton instances currently registered
        /// </summary>
        protected IDictionary<Type, object> singletonInstances = new Dictionary<Type, object>();

        /// <summary>
        /// The list of current singleton types currently registered
        /// </summary>
        protected IDictionary<Type, Type> singletonTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// The list of current scoped instances currently registered
        /// </summary>
        protected readonly IDictionary<Type, object> scopedInstances = new Dictionary<Type, object>();

        private readonly List<Type> currentTypes = new List<Type>();
        private Type currentType;

        private bool isScope = false;
        private bool canPrintToConsole = false;

        /// <summary>
        /// Adds an implementation as a singleton on the container.
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <param name="implementationType">The type that will be added</param>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton(Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.singletonTypes[implementationType] = implementationType;
        }

        /// <summary>
        /// Adds an implementation as a singleton on the container.
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            this.AddSingleton(implementationType);
        }

        /// <summary>
        /// Adds an instance as a singleton on the container
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="instance"></param>
        public void AddSingleton<TImplementation>(TImplementation instance) where TImplementation : class
        {
            this.singletonInstances[typeof(TImplementation)] = instance;
        }

        /// <summary>
        /// Adds an implementation to a contract as a singleton on the container
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <param name="contractType">Contract type</param>
        /// <param name="implementationType">Implementation type</param>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton(Type contractType, Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.singletonTypes[contractType] = implementationType;
        }

        /// <summary>
        /// Adds an implementation to a contract as a singleton on the container
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton<TContract, TImplementation>() where TImplementation : class, TContract
        {
            var contractType = typeof(TContract);
            var implementationType = typeof(TImplementation);
            AddSingleton(contractType, implementationType);
        }

        /// <summary>
        /// Adds an instance as a singleton on the container
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void AddSingleton<TContract, TImplementation>(TImplementation instance) where TImplementation : TContract
        {
            this.singletonInstances[typeof(TContract)] = instance;
        }

        /// <summary>
        /// Adds a scoped implementation to a contract on the container.
        /// </summary>
        public void AddScoped(Type contractType, Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.scopedTypes[contractType] = implementationType;
        }

        /// <summary>
        /// Adds a scoped implementation to a contract on the container.
        /// </summary>
        public void AddScoped<TContract, TImplementation>() where TImplementation : class, TContract
        {
            var implementationType = typeof(TImplementation);
            var contractType = typeof(TContract);
            this.AddScoped(contractType, implementationType);
        }

        /// <summary>
        /// Adds a scoped implementation on the container.
        /// </summary>
        public void AddScoped(Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.scopedTypes[implementationType] = implementationType;
        }

        /// <summary>
        /// Adds a scoped implementation on the container.
        /// </summary>
        public void AddScoped<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            this.AddScoped(implementationType);
        }

        /// <summary>
        /// Adds an implementation as transient on the container
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <type name="implementationType"></type>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddTransient(Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.transientTypes[implementationType] = implementationType;
        }

        /// <summary>
        /// Adds an implementation as a transient on the container.
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddTransient<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            this.AddTransient(implementationType);
        }

        /// <summary>
        /// Adds an implementation to a contract as transient on the container
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <type name="contractType"></type>
        /// <type name="implementationType"></type>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddTransient(Type contractType, Type implementationType)
        {
            EnsureNonAbstractClass(implementationType);
            this.transientTypes[contractType] = implementationType;
        }

        /// <summary>
        /// Adds an implementation to a contract as transient on the container
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddTransient<TContract, TImplementation>() where TImplementation : class, TContract
        {
            var contractType = typeof(TContract);
            var implementationType = typeof(TImplementation);
            this.AddTransient(contractType, implementationType);
        }

        /// <summary>
        /// Checks if a singleton instance for this type is registered in the container
        /// </summary>
        /// <typeparam name="T">The target type, abstract or concrete</typeparam>
        /// <returns>true if the type is registered, false if not</returns>
        public bool HasSingletonInstance<T>()
        {
            return this.singletonInstances.ContainsKey(typeof(T)) || this.singletonTypes.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Checks if a scoped instance for this type is registered in the container
        /// </summary>
        /// <typeparam name="T">The target type, abstract or concrete</typeparam>
        /// <returns>true if the type is registered, false if not</returns>
        public bool HasScopedInstance<T>()
        {
            return this.scopedInstances.ContainsKey(typeof(T)) || this.scopedTypes.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Asks the container for a new instance of a type
        /// <br>It will return null if any non registered abstract type in the dependency tree</br>
        /// </summary>
        public T ResolveOrNull<T>() where T : class
        {
            try
            {
                return this.Resolve<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        public T Resolve<T>() where T : class
        {
            var instance = (T)this.ResolveObject(typeof(T));
            this.currentTypes.Clear();
            return instance;
        }

        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        public object Resolve(Type type)
        {
            var instance = this.ResolveObject(type);
            this.currentTypes.Clear();
            return instance;
        }

        private object ResolveObject(Type contract)
        {
            if (this.canPrintToConsole)
            {
                Console.WriteLine("Onbox Container request: " + contract.ToString());
            }

            // Always prioritize instances
            if (this.singletonInstances.ContainsKey(contract))
            {
                return this.singletonInstances[contract];
            }
            else if (this.singletonTypes.ContainsKey(contract))
            {
                var instance = this.ResolveObject(contract, this.singletonTypes);
                this.singletonInstances[contract] = instance;
                this.singletonTypes.Remove(contract);
                return instance;
            }
            if (this.scopedInstances.ContainsKey(contract))
            {
                return this.scopedInstances[contract];
            }
            else if (this.scopedTypes.ContainsKey(contract))
            {
                var instance = this.ResolveObject(contract, this.scopedTypes);
                this.scopedInstances[contract] = instance;
                return instance;
            }
            else if (!contract.IsAbstract || this.transientTypes.ContainsKey(contract))
            {
                var instance = this.ResolveObject(contract, this.transientTypes);
                return instance;
            }
            else
            {
                throw new KeyNotFoundException($"{contract.Name} not registered on Onbox Container.");
            }
        }

        private object ResolveObject(Type contract, IDictionary<Type, Type> dic)
        {
            // Check for Ciruclar dependencies
            CheckForCircularDependencies(contract);

            // If this is a concrete type just instantiate it, if not, get the concrete type on the dictionary
            Type implementation = contract;
            if (implementation.IsAbstract)
            {
                implementation = dic[contract];
            }

            this.currentTypes.Add(implementation);

            // Get the first available contructor
            var constructors = implementation.GetConstructors();
            if (constructors.Length < 1)
            {
                throw new InvalidOperationException($"Onbox Container: {implementation.Name} has no available constructors. The container can not instantiate it.");
            }

            ConstructorInfo constructor = constructors[0];

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                if (this.canPrintToConsole)
                {
                    Console.WriteLine("Onbox Container instantiated " + implementation.ToString());
                }
                currentTypes.Remove(implementation);
                return Activator.CreateInstance(implementation);
            }
            List<object> parameters = new List<object>(constructorParameters.Length);
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                var type = parameterInfo.ParameterType;
                currentType = implementation;
                parameters.Add(this.ResolveObject(type));
            }

            if (this.canPrintToConsole)
            {
                Console.WriteLine("Onbox Container instantiated " + implementation.ToString());
            }
            currentTypes.Remove(implementation);
            return constructor.Invoke(parameters.ToArray());
        }

        private void CheckForCircularDependencies(Type contract)
        {
            if (this.currentTypes.Contains(contract))
            {
                string error;
                if (currentType == contract)
                {
                    error = $"Onbox Container found circular dependency on {currentType.Name} trying to inject itself.";
                    throw new InvalidOperationException(error);
                }

                error = $"Onbox Container found circular dependency between {currentType.Name} and {contract.Name}.";
                throw new InvalidOperationException(error);
            }
        }

        /// <summary>
        /// Clears and releases resources from the container
        /// </summary>
        public void Clear()
        {
            this.transientTypes.Clear();
            this.scopedInstances.Clear();
            this.scopedTypes.Clear();
            this.currentTypes.Clear();
            this.currentType = null;

            // If this is a scope we can not clean the singletons as they have the same reference as the main container
            if (!isScope)
            {
                this.singletonTypes.Clear();
                this.singletonInstances.Clear();
            }
        }

        private static void EnsureNonAbstractClass(Type type)
        {
            if (type.IsInterface)
            {
                throw new InvalidOperationException($"Can not add {type.Name} to Onbox Container because it is an interface. You need to provide a concrete type or provide an instance.");
            }
            if (type.IsAbstract)
            {
                throw new InvalidOperationException($"Can not add {type.Name} to Onbox Container because because it is an abstract type. You need to provide a concrete type or provide an instance.");
            }
        }

        /// <summary>
        /// Clears and releases resources from the container
        /// </summary>
        public void Dispose()
        {
            if (this.canPrintToConsole)
            {
                Console.WriteLine("Onbox Container disposing... ");
            }
            Clear();
        }

        /// <summary>
        /// Creates a scoped context copy of this container
        /// </summary>
        public IContainerResolver CreateScope()
        {
            // Creates a copy of the Container with the relevant types and instances
            var container = new Container();
            container.isScope = true;

            container.transientTypes = this.transientTypes.ToDictionary(k => k.Key, v => v.Value);
            container.scopedTypes = this.scopedTypes.ToDictionary(k => k.Key, v => v.Value);

            // References the same singletons as the main container
            container.singletonInstances = this.singletonInstances;
            container.singletonTypes = this.singletonTypes;

            // This will override the singleton instances for the scoped ones
            container.AddSingleton<IContainerResolver>(container);
            container.AddSingleton<IContainer>(container);
            return container;
        }

        /// <summary>
        /// Reports if this container is a scope of a container
        /// </summary>
        public bool IsScope()
        {
            return this.isScope;
        }

        /// <summary>
        /// Enables or disables the container for printing to the console when events happen. Default is false.
        /// <br/> * Requesting for new instances
        /// <br/> * Instantiating
        /// <br/> * Disposing
        /// </summary>
        /// <param name="enabled">flag to enable or disable console priting.</param>
        public void EnableConsolePrinting(bool enabled)
        {
            this.canPrintToConsole = enabled;
        }
    }
}