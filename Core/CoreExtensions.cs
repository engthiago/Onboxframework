using Onbox.Core.V1.Http;
using Onbox.Core.V1.Json;
using Onbox.Di.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1
{
    public static class CoreExtensions
    {
        /// <summary>
        /// Adds HttpClientService and JsonService to the container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Container AddOnboxCore(this Container container)
        {
            container.AddHttp();
            container.AddJson();

            return container;
        }
    }
}
