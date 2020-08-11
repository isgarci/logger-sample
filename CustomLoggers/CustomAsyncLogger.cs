using System;
using Serilog;
using Logger.Model;
using Logger.CustomLoggers.Config;
using System.Threading.Tasks;
using System.Threading;

namespace Logger.CustomLoggers
{
    public class CustomAsyncLogger : ILogger
    {
        private Serilog.ILogger _logger;   
        private CustomFileConfig _customConfig;
        private CancellationTokenSource _taskCancelToken;
        private Task _task;
        public DateTime _loggerCreationDate { get; set; }
        public string _logFileName { get; set; }

        public CustomAsyncLogger()
        {
        }

        public void setCustomConfig(CustomFileConfig customConfig)
        {
            this._customConfig = customConfig;
            this._logger = createNewLogFile();
        }

        public void runTask(Task t, CancellationTokenSource ts = null)
        {
                this._task = t;
                this._taskCancelToken = ts;
                this._task.Start();
        }

        public void StopWithFlush()
        {
            this._task.Wait();
            this._task.Dispose();
        }

        public void StopWithoutFlush()
        {
            if(this._taskCancelToken == null)
            {
                throw new AggregateException("Task can not be canceled.");
            }

            this._taskCancelToken.Cancel();
        }

        public void WriteLog(ILogEntry logEntry)
        {
            DateTime currentDate = DateTime.Now;
            if ((currentDate - this._loggerCreationDate).Days != 0)
            {
                this._logger = createNewLogFile();
            }
            this._logger.Information("{LogMessage}", logEntry._logMessage); 
        }

        private Serilog.ILogger createNewLogFile()
        {

            if (_customConfig == null)
            {
                throw new ApplicationException("Custom Config not defined");
            }

            this._loggerCreationDate = DateTime.Now;
            this._logFileName = this._customConfig._logPath + "/" +
                this._loggerCreationDate.ToString(this._customConfig._FILE_NAME_FORMAT) +
                ".log";

            return new LoggerConfiguration().
                WriteTo.File(this._logFileName).CreateLogger();
        }

        public Action<object> WriteLines(
                    int line, Func<int, bool> condition, bool desc)
        {

            Action<object> logFileAction = (object obj) =>
            {
                ILogger logger = (ILogger)obj;

                while (condition(line))
                {
                    LogEntry logEntry = new LogEntry(line.ToString(), DateTime.Now);
                    logger.WriteLog(logEntry);
                    line = desc ? --line : ++line;
                }
            };

            return logFileAction;

        }


        public Task<int> WriteLines(
            int line, Func<int, bool> condition, bool desc,
            CancellationToken cancellationToken )
        {

            Task<int> task = null;

            task = Task.Run(() =>
            {
                while (condition(line))
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new TaskCanceledException(task);

                    LogEntry logEntry = new LogEntry(line.ToString(), DateTime.Now);
                    WriteLog(logEntry);
                    line = desc ? --line : ++line;
                }
                return line;
            });

            return task;
        }

    }
}
