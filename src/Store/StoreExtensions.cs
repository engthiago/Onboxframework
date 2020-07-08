using Onbox.Abstractions.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Store.V7
{
    public static class StoreExtensions
    {
        /// <summary>
        /// Adds a Store of <typeparamref name="T"/> which can be injected as IStore of <typeparamref name="T"/> in services
        /// </summary>
        public static IContainer AddStore<T>(this IContainer container) where T : class, new()
        {
            container.AddSingleton<IStore<T>, Store<T>>();

            return container;
        }
    }
}
