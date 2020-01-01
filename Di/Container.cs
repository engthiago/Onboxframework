using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Di.V1
{
    public class Container
    {
        private static readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();
        private static readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        public void AddTransient<TContract, TImplementation>()
        {
            types[typeof(TContract)] = typeof(TImplementation);
        }

        public void AddTransient<TImplementation>()
        {
            types[typeof(TImplementation)] = typeof(TImplementation);
        }

        public void AddSingleton<TImplementation>(TImplementation instance)
        {
            instances[typeof(TImplementation)] = instance;
        }

        public void AddSingleton<TContract, TImplementation>(TImplementation instance)
        {
            instances[typeof(TContract)] = instance;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type contract)
        {
            if (instances.ContainsKey(contract))
            {
                return instances[contract];
            }
            else
            {
                Type implementation = types[contract];
                ConstructorInfo constructor = implementation.GetConstructors()[0];
                ParameterInfo[] constructorParameters = constructor.GetParameters();
                if (constructorParameters.Length == 0)
                {
                    return Activator.CreateInstance(implementation);
                }
                List<object> parameters = new List<object>(constructorParameters.Length);
                foreach (ParameterInfo parameterInfo in constructorParameters)
                {
                    parameters.Add(Resolve(parameterInfo.ParameterType));
                }
                return constructor.Invoke(parameters.ToArray());
            }
        }

        public void Reset()
        {
            types.Clear();
            instances.Clear();
        }
    }
}
