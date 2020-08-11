using System;
using System.Text;

namespace Logger.Model
{
    public class LogEntry : ILogEntry
    {

        #region Properties
        /// <summary>
        /// The text to be display in logline
        /// </summary>
        public string _logMessage { get; set; }

        /// <summary>
        /// The Timestamp at the moment when the log is added. 
        /// </summary>
        public DateTime _timestamp { get; set; }
        #endregion 


        public LogEntry()
        {
        }

        public LogEntry(string logMessage, DateTime timestamp)
        {
            this._logMessage = logMessage;
            this._timestamp = timestamp;
        }

    }
}
