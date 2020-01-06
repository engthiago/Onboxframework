using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1.Logging
{
    public class FileLoggingService : ILoggingService
    {
        private readonly FileLoggingSettings settings;

        public FileLoggingService(FileLoggingSettings settings)
        {
            this.settings = settings;
        }

        public async Task Error(string message)
        {
            var type = "Error";
            await this.Log(message, type);
        }

        public async Task Log(string message)
        {
            var type = "Log";
            await this.Log(message, type);
        }

        public async Task Warning(string message)
        {
            var type = "Warning";
            await this.Log(message, type);
        }


        private async Task Log(string message, string type)
        {
            try
            {
                var fileInfo = new FileInfo(this.settings.FullPathName);
                if (fileInfo.Exists && fileInfo.Length > settings.MaxFileSizeInBytes)
                {
                    fileInfo.Delete();
                }

                using (var sw = File.AppendText(this.settings.FullPathName))
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

    public class FileLoggingSettings
    {
        public bool CanThrowExceptions { get; set; }
        public string FullPathName { get; set; }
        public long MaxFileSizeInBytes { get; set; }
    }
}
