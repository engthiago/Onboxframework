using Onbox.Core.V1.Http;
using Onbox.Core.V1.Json;
using Onbox.Core.V1.Logging;
using Onbox.Di.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1
{
    /// <summary>
    /// Core extensions for Onbox's container
    /// </summary>
    public static class CoreExtensions
    {
        /// <summary>
        /// Adds <see cref="IHttpService"/> and <see cref="IJsonService"/> default implementation to the container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Container AddOnboxCore(this Container container)
        {
            container.AddHttp();
            container.AddJson();

            return container;
        }

        /// <summary>
        /// Adds <see cref="ILoggingService"/> as <see cref="FileLoggingService"/> to the container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="config">If no configuration is specified it will log to the user's temp folder with a maximum size of 200kb</param>
        /// <returns></returns>
        public static Container AddFileLogging(this Container container, Action<FileLoggingSettings> config = null)
        {
            // The default settings for file settings
            var settings = new FileLoggingSettings
            {
                CanThrowExceptions = false,
                FullPathName = $"{Path.GetTempPath()}/{System.Reflection.Assembly.GetCallingAssembly().GetName().Name}.log",
                MaxFileSizeInBytes = 100
            };

            config?.Invoke(settings);

            container.AddSingleton(settings);
            container.AddSingleton<ILoggingService, FileLoggingService>();

            return container;
        }
    }
}
