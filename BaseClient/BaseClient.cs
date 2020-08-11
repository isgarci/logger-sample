using System;
using Logger.CustomLoggers;
using Logger.CustomLoggers.Config;
using System.Threading.Tasks;
using System.Threading;

namespace Logger.BaseClient
{
    public class BaseClient
    {
        public static void Main(string[] args)
        {
            CustomFileConfig localConf = new CustomFileConfig();

            CustomAsyncLogger logger1 = new CustomAsyncLogger();
            logger1.setCustomConfig(localConf);
            logFile1(logger1);

            CustomAsyncLogger logger2 = new CustomAsyncLogger();
            logger2.setCustomConfig(localConf);
            logFile2(logger2);
        }

        static void logFile1(CustomAsyncLogger logger)
        {
            int line = 0;
            Action<object> logFileAction = logger.WriteLines(line,
                           x => x < 15, false);

            Task t = new Task(logFileAction, logger);
            logger.runTask(t);

            try
            {
                logger.StopWithFlush();
                Console.WriteLine("t1 Status: {0}", t.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in logger with flush.");
            }
        }


        static async void logFile2(CustomAsyncLogger logger)
        {
            int maxLines = 50;
            bool inDescendantOrder = true;

            DateTime loggingStartDate = DateTime.Now;

            using (var cts = new CancellationTokenSource(3000))
            {
                try
                {
                    var result = await logger.WriteLines(
                            maxLines, x => x > 0, inDescendantOrder, cts.Token);

                    Console.WriteLine("t2 result: {0}", result);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }
            }

        }

    }

}