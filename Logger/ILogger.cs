using System;
using Logger.Model;

namespace Logger
{
    public interface ILogger {

        /// <summary>
        /// Stop the logging. If any outstanding logs, these will not be written to Log
        /// </summary>
        void StopWithoutFlush();

        /// <summary>
        /// Stop the logging. The call will not return until all logs have been written to Log.
        /// </summary>
        void StopWithFlush();

        /// <summary>
        /// WriteLog a message to the Log.
        /// </summary>
        /// <param name="logEntry">The logEntry to write at the log</param>
        void WriteLog(ILogEntry logEntry);
    }
}
