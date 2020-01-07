using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1.Logging
{
    /// <summary>
    /// Logs to a local file
    /// </summary>
    public class FileLoggingService : ILoggingService
    {
        private readonly FileLoggingSettings settings;

        public FileLoggingService(FileLoggingSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Logs an error to the .log file
        /// </summary>
        public async Task Error(string message)
        {
            var type = "Error";
            await this.Log(message, type);
        }

        /// <summary>
        /// Logs a message to the .log file
        /// </summary>
        public async Task Log(string message)
        {
            var type = "Log";
            await this.Log(message, type);
        }

        /// <summary>
        /// Logs a warning to the .log file
        /// </summary>
        public async Task Warning(string message)
        {
            var type = "Warning";
            await this.Log(message, type);
        }


        private async Task Log(string message, string type)
        {
            try
            {
                var fullPath = Path.Combine(this.settings.Path, this.settings.FileName);

                var fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists && fileInfo.Length > settings.MaxFileSizeInBytes)
                {
                    fileInfo.Delete();
                }

                using (var sw = File.AppendText(fullPath))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: *** {type} ***: {message}");
                }
            }
            catch (Exception e)
            {
                if (this.settings.CanThrowExceptions)
                {
                    throw e;
                }
            }
        }
    }

    /// <summary>
    /// The settings used by <see cref="FileLoggingService"/>
    /// </summary>
    public class FileLoggingSettings
    {
        /// <summary>
        /// [Default = false] If set to true, FileLoggingService will throw exceptions if it runs into errors
        /// </summary>
        public bool CanThrowExceptions { get; set; }
        /// <summary>
        /// [Default = %temp%] The full directory path where the file will be saved
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// [Default = 200000] The maxium size of the logging file, default is 200000 which means 200kb  
        /// </summary>
        public long? MaxFileSizeInBytes { get; set; }
        /// <summary>
        /// [Default = CallingAssembly] The name of the file used for logging, it will append .log to the filename if not provided
        /// </summary>
        public string FileName { get; set; }
    }
}
