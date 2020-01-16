using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1.Http
{
    public class HttpSettings
    {
        /// <summary>
        /// Timeout in milliseconds
        /// </summary>
        public int Timeout { get; set; } = 2500;

        /// <summary>
        /// The "cache-control" header will be set to no-cache by default
        /// </summary>
        public bool AllowCache { get; set; }
    }
}
