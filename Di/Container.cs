using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Di.V1
{
    public interface IContainer
    {
        void AddSingleton<TImplementation>() where TImplementation: class;
        void AddSingleton<TContract>(TContract instance) where TContract : class;
        void AddSingleton<TContract, TImplementation>() where TImplementation : TContract;
        void AddSingleton<TContract, TImplementation>(TImplementation instance) where TImplementation : TContract;

        void AddTransient<TImplementation>() where TImplementation : class;
        void AddTransient<TContract, TImplementation>() where TImplementation : TContract;

        void Reset();
        T Resolve<T>();
    }

    public class Container : IContainer
    {
        private readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        private readonly IDictionary<Type, Type> singletonCache = new Dictionary<Type, Type>();

        private List<Type> currentTypes = new List<Type>();
        private Type currentType;

        public void AddSingleton<TImplementation>() where TImplementation : class
        {
            var type = typeof(TImplementation);
            CheckForValidNonAbstractClass(type);
            this.singletonCache[type] = type;
        }

        public void AddSingleton<TContract>(TContract instance) where TContract : class
        {
            this.instances[typeof(TContract)] = instance;
        }

        public void AddSingleton<TContract, TImplementation>() where TImplementation : TContract
        {
            var type = typeof(TImplementation);
            CheckForValidNonAbstractClass(type);
            this.singletonCache[typeof(TContract)] = type;
        }

        public void AddSingleton<TContract, TImplementation>(TImplementation instance) where TImplementation : TContract
        {
            this.instances[typeof(TContract)] = instance;
        }



        public void AddTransient<TImplementation>() where TImplementation : class
        {
            var type = typeof(TImplementation);
            CheckForValidNonAbstractClass(type);
            this.types[type] = type;
        }

        public void AddTransient<TContract, TImplementation>() where TImplementation : TContract
        {
            var type = typeof(TImplementation);
            CheckForValidNonAbstractClass(type);
            this.types[typeof(TContract)] = type;
        }



        public T Resolve<T>()
        {
            var type = (T)this.Resolve(typeof(T));
            this.currentTypes.Clear();
            return type;
        }

        private object Resolve(Type contract)
        {
            // Always prioritize instances
            if (this.instances.ContainsKey(contract))
            {
                return this.instances[contract];
            }
            else if (this.singletonCache.ContainsKey(contract))
            {
                var instance = this.Resolve(contract, this.singletonCache);
                this.instances[contract] = instance;
                this.singletonCache.Remove(contract);
                return instance;
            }
            else if (!contract.IsAbstract || this.types.ContainsKey(contract))
            {
                return this.Resolve(contract, this.types);
            }
            else
            {
                throw new KeyNotFoundException($"{contract.Name} not registered on the DI container.");
            }
        }


        private object Resolve(Type contract, IDictionary<Type, Type> dic)
        {
            // Check for Ciruclar dependencies
            if (this.currentTypes.Contains(contract))
            {
                throw new InvalidOperationException($"Circular dependency between: {contract.Name} and {this.currentType.Name}.");
            }
            this.currentTypes.Add(contract);
            this.currentType = contract;

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
                throw new InvalidOperationException($"{implementation.Name} has no available constructors. The container can not instantiate it.");
            }

            ConstructorInfo constructor = constructors[0];

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }
            List<object> parameters = new List<object>(constructorParameters.Length);
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(this.Resolve(parameterInfo.ParameterType));
            }
            return constructor.Invoke(parameters.ToArray());
        }

        public void Reset()
        {
            this.types.Clear();
            this.instances.Clear();
            this.singletonCache.Clear();
        }

        public static Container Default()
        {
            var container = new Container();
            container.AddSingleton<IContainer>(container);

            return container;
        }

        private static void CheckForValidNonAbstractClass(Type type)
        {
            if (type.IsInterface)
            {
                throw new InvalidOperationException($"Can not add {type.Name} to the container because it is an interface. You need to provide a concrete type or provide an instance.");
            }
            if (type.IsAbstract)
            {
                throw new InvalidOperationException($"Can not add {type.Name} to the container because because it is an abstract type. You need to provide a concrete type or provide an instance.");
            }
        }
    }
}
