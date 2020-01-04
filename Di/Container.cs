using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Di.V1
{
    public interface IContainer
    {
        void AddSingleton<TImplementation>();
        void AddSingleton<TImplementation>(TImplementation instance);
        void AddSingleton<TContract, TImplementation>() where TImplementation : TContract;
        void AddSingleton<TContract, TImplementation>(TImplementation instance) where TImplementation : TContract;

        void AddTransient<TImplementation>();
        void AddTransient<TContract, TImplementation>() where TImplementation : TContract;

        void Reset();
        T Resolve<T>();
    }

    public class Container : IContainer
    {
        private static readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();
        private static readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        private static readonly IDictionary<Type, Type> singletonCache = new Dictionary<Type, Type>();

        private List<Type> currentTypes = new List<Type>();
        private Type currentType;

        public void AddSingleton<TImplementation>()
        {
            singletonCache[typeof(TImplementation)] = typeof(TImplementation);
        }

        public void AddSingleton<TImplementation>(TImplementation instance)
        {
            instances[typeof(TImplementation)] = instance;
        }

        public void AddSingleton<TContract, TImplementation>() where TImplementation : TContract
        {
            singletonCache[typeof(TContract)] = typeof(TImplementation);
        }

        public void AddSingleton<TContract, TImplementation>(TImplementation instance) where TImplementation : TContract
        {
            instances[typeof(TContract)] = instance;
        }



        public void AddTransient<TImplementation>()
        {
            types[typeof(TImplementation)] = typeof(TImplementation);
        }

        public void AddTransient<TContract, TImplementation>() where TImplementation : TContract
        {
            types[typeof(TContract)] = typeof(TImplementation);
        }



        public T Resolve<T>()
        {
            var type = (T)this.Resolve(typeof(T));
            currentTypes.Clear();
            return type;
        }

        private object Resolve(Type contract)
        {
            if (instances.ContainsKey(contract))
            {
                return instances[contract];
            }
            else if (singletonCache.ContainsKey(contract))
            {
                var instance = this.Resolve(contract, singletonCache);
                instances[contract] = instance;
                singletonCache.Remove(contract);
                return instance;
            }
            else if (types.ContainsKey(contract))
            {
                return this.Resolve(contract, types);
            }
            else
            {
                throw new KeyNotFoundException($"{contract.Name} not registered on the DI container.");
            }
        }


        private object Resolve(Type contract, IDictionary<Type, Type> dic)
        {
            // Check for Ciruclar dependencies
            if (currentTypes.Contains(contract))
            {
                throw new InvalidOperationException($"Circular dependency between: {contract.Name} and {currentType.Name}.");
            }
            currentTypes.Add(contract);
            currentType = contract;

            // Get Type from the dictionary
            Type implementation = dic[contract];

            // Get the first available contructor
            ConstructorInfo constructor = implementation.GetConstructors()[0];
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
            types.Clear();
            instances.Clear();
            singletonCache.Clear();
        }

        public static Container Default()
        {
            var container = new Container();
            container.AddSingleton<IContainer>(container);

            return container;
        }
    }
}
