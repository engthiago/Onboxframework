using System;

namespace Onbox.Core.V7.Mapping
{
    public interface IMapperConfigurator
    {
        void AddMappingPostAction<TSource, TTaget>(Action<TSource, TTaget> action) where TSource : new() where TTaget : new();
        Delegate GetMapFunction(Type source, Type target);
    }
}