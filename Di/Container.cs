using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Di.V7
{
    /// <summary>
    /// Onbox's IOC container implementation
    /// </summary>
    public class Container : IContainer, IDisposable
    {
        private IDictionary<Type, Type> transientTypes = new Dictionary<Type, Type>();

        private IDictionary<Type, Type> scopedTypes = new Dictionary<Type, Type>();

        private IDictionary<Type, object> singletonInstances = new Dictionary<Type, object>();
        private IDictionary<Type, Type> singletonTypes = new Dictionary<Type, Type>();


        private readonly IDictionary<Type, object> scopedInstances = new Dictionary<Type, object>();
        private readonly List<Type> currentTypes = new List<Type>();
        private Type currentType;


        /// <summary>
        /// Adds an implementation as a singleton on the container.
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton<TImplementation>() where TImplementation : class
        {
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.singletonTypes[type] = type;
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
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddSingleton<TContract, TImplementation>() where TImplementation : class, TContract
        {
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.singletonTypes[typeof(TContract)] = type;
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
        /// Adds an implementation as a transient on the container.
        /// </summary>
        /// <remarks>It can not be an abstract or interface type</remarks>
        /// <typeparam name="TImplementation"></typeparam>
        /// <exception cref="InvalidOperationException">Thrown when using a abstract class or an interface</exception>
        public void AddTransient<TImplementation>() where TImplementation : class
        {
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.transientTypes[type] = type;
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
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.transientTypes[typeof(TContract)] = type;
        }

        /// <summary>
        /// Adds a scoped implementation to a contract on the container.
        /// </summary>
        public void AddScoped<TContract, TImplementation>() where TImplementation : class, TContract
        {
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.scopedTypes[typeof(TContract)] = type;
        }

        /// <summary>
        /// Adds a scoped implementation on the container.
        /// </summary>
        public void AddScoped<TImplementation>() where TImplementation : class
        {
            var type = typeof(TImplementation);
            EnsureNonAbstractClass(type);
            this.scopedTypes[type] = type;
        }


        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        public T Resolve<T>()
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
            Console.WriteLine("Onbox Container request: " + contract.ToString());

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
            if (this.currentTypes.Contains(contract))
            {
                string error;
                if (currentType == contract)
                {
                    error = $"Onbox Container found circular dependency on {currentType.Name} trying to inject itself.";
                    Console.WriteLine(error);
                    throw new InvalidOperationException(error);
                }

                if (currentType != null)
                {
                    error = $"Onbox Container found circular dependency between {currentType.Name} and {contract.Name}.";
                    Console.WriteLine(error);
                    throw new InvalidOperationException(error);
                }

                error = $"Onbox Container found circular dependency on: {contract.Name}.";
                Console.WriteLine(error);
                throw new InvalidOperationException(error);
            }
            this.currentTypes.Add(contract);

            // If this is a concrete type just instantiate it, if not, get the concrete type on the dictionary
            Type implementation = contract;
            if (implementation.IsAbstract)
            {
                implementation = dic[contract];
            }

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
                Console.WriteLine("Onbox Container instantiated " + implementation.ToString());
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

            Console.WriteLine("Onbox Container instantiated " + implementation.ToString());
            currentTypes.Remove(implementation);
            return constructor.Invoke(parameters.ToArray());
        }

        /// <summary>
        /// Clears and releases resources from the container
        /// </summary>
        public void Clear()
        {
            this.transientTypes?.Clear();
            this.singletonInstances?.Clear();
            this.singletonTypes?.Clear();
            this.scopedInstances?.Clear();
            this.scopedTypes?.Clear();
            this.currentTypes?.Clear();
            this.currentType = null;
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
            Console.WriteLine("Onbox Container disposing... ");
            Clear();
        }

        /// <summary>
        /// Creates a scoped context copy of this container
        /// </summary>
        public IContainerResolver CreateScope()
        {
            // Creates a copy of the Container with the relevant types and instances
            var container = new Container();
            container.transientTypes = this.transientTypes.ToDictionary(k => k.Key, v => v.Value);
            container.singletonInstances = this.singletonInstances.ToDictionary(k => k.Key, v => v.Value);
            container.singletonTypes = this.singletonTypes.ToDictionary(k => k.Key, v => v.Value);
            container.scopedTypes = this.scopedTypes.ToDictionary(k => k.Key, v => v.Value);

            // This will override the singleton instances for the scoped ones
            container.AddSingleton<IContainerResolver>(container);
            container.AddSingleton<IContainer>(container);
            return container;
        }
    }
}
