using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.VDev.Logging
{
    /// <summary>
    /// Logs to a local file
    /// </summary>
    public class FileLoggingService : ILoggingService
    {
        private readonly FileLoggingSettings settings;

        /// <summary>
        /// Constructor
        /// </summary>
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
        /// Logs an exception to the .log file
        /// </summary>
        public async Task Exception(Exception exception)
        {
            var type = "Exception";
            await this.Log(exception?.Message, type);
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
                var fullPath = this.settings.FileName;
                if (!this.settings.SaveOnCurrentPath)
                {
                    if (!Directory.Exists(this.settings.Path))
                    {
                        Directory.CreateDirectory(this.settings.Path);
                    }
                    fullPath = Path.Combine(this.settings.Path, this.settings.FileName);
                }

                var fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists && fileInfo.Length > this.settings.MaxFileSizeInBytes)
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
        /// [Default = %temp%] The full directory path where the file will be saved, if <see cref="SaveOnCurrentPath"/> = true, this will be ignored
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// [Default = 600000] The maxium size of the logging file, default is 600000 which means 600kb  
        /// </summary>
        public long? MaxFileSizeInBytes { get; set; }
        /// <summary>
        /// [Default = Onbox.Logging.log] The name of the file used for logging, it will append .log to the filename if not provided
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// [Default = false] This will ignore the <see cref="Path"/> property and will save the log in the current directory, USEFUL for RevitIO
        /// <para>WARNING: Setting it to true on a normal desktop scenario should not work because it will try to save it on Revit's main directory</para>
        /// </summary>
        public bool SaveOnCurrentPath { get; set; }
    }
}