using System;
using Logger.Model;

namespace Logger
{
    public class Logger
    {
        private ILogger logger;

        public Logger(ILogger logger)
        {
            this.logger = logger;
        }

        public void StopWithFlush()
        {
            this.logger.StopWithFlush();
        }

        public void StopWithoutFlush()
        {
            this.logger.StopWithoutFlush();
        }
        
        public void WriteLog(ILogEntry logEntry)
        {
            this.logger.WriteLog(logEntry);
        }

    }

}




