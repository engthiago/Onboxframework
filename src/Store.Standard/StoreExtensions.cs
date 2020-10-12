using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Store.VDev
{
    public static class StoreExtensions
    {
        /// <summary>
        /// Adds a Store of <typeparamref name="T"/> which can be injected as IStore of <typeparamref name="T"/> in services
        /// </summary>
        public static IContainer AddStore<T>(this IContainer container) where T : class, new()
        {
            var store = container.Resolve<Store<T>>();
            container.AddSingleton<IStore<T>, Store<T>>(store);

            return container;
        }
    }
}