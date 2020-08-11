using System;
using System.IO;
using Logger.Config;

namespace Logger.CustomLoggers.Config
{
    public class CustomFileConfig : ILogConfig
    {
        public string _LOCAL_DIR_NAME { get; set; } = "LogTest";
        public string _FILE_NAME_FORMAT { get; set; } = "yyyyMMdd HHmmss fff";
        public int _CANCEL_TIME { get; set; } = 3000;
        public string _logPath { get; set; }
        
        public CustomFileConfig()
        {
            configDefaultLogPath(_LOCAL_DIR_NAME);
        }

        private void configDefaultLogPath(string logDirName)
        {
            this._logPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                                + "/" + logDirName;

            if (!Directory.Exists(this._logPath))
            {
                Directory.CreateDirectory(this._logPath);
            }
        }

    }
}
