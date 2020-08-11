using System;

namespace Logger.Model
{
    public interface ILogEntry
    {
        string _logMessage { get; set; }
        DateTime _timestamp { get; set; }
    }
}
