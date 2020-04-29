using System;
using System.Collections.Generic;
using System.Text;

namespace Onbox.Standard.Core.Http
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
