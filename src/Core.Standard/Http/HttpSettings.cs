using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.VDev.Http
{
    /// <summary>
    /// Preferences for the default implementation of <see cref="IHttpService"/>
    /// </summary>
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