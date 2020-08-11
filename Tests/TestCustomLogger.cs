using NUnit.Framework;
using System;
using Logger.CustomLoggers;
using Logger.CustomLoggers.Config;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Logger.Tests
{
    [TestFixture()]
    public class TestCustomLogger
    {
        CustomFileConfig localConf;
        CustomAsyncLogger logger;

        [SetUp]
        public void Setup()
        {
            
            localConf = new CustomFileConfig();
            localConf._logPath =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName 
            + "/LogOutputSample";

            if (!Directory.Exists(localConf._logPath))
            {
                Directory.CreateDirectory(localConf._logPath);
            }

            logger = new CustomAsyncLogger();
            logger.setCustomConfig(localConf);

        }

        [Test()]
        public void TestLoggerWithFlush()
        {

            int line = 0;
            int maxLines = 15;
            bool inDescendantOrder = false;
            Action<object> logLines = logger.WriteLines(
                            line, x => x < maxLines, inDescendantOrder);

            Task t = new Task(logLines, logger);

            DateTime loggingStartDate = DateTime.Now;

            logger.runTask(t);
            logger.StopWithFlush();

            if ((loggingStartDate - logger._loggerCreationDate).Days == 0)
            {
                var lineCount = File.ReadAllLines(logger._logFileName).Length;
                Assert.AreEqual(maxLines, lineCount);
            }
            else
            {
                TestDayShift(loggingStartDate, maxLines);
            }
        }


        [Test()]
        public async Task TestLoggerWithoutFlush()
        {

            localConf._CANCEL_TIME = 20;

            int maxLines = 50;
            bool inDescendantOrder = true;

            DateTime loggingStartDate = DateTime.Now;
            using (var cts = new CancellationTokenSource(localConf._CANCEL_TIME))
            {
                try
                {
                    var result = await logger.WriteLines(
                            maxLines, x => x > 0, inDescendantOrder, cts.Token);
                    Console.WriteLine("Result {0}", result);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }
            }


            if ((loggingStartDate - logger._loggerCreationDate).Days == 0)
            {
                var lineCount = File.ReadAllLines(logger._logFileName).Length;
                Assert.LessOrEqual(lineCount, maxLines);
            }
            else   // Shift day scenario
            {
                TestDayShift(loggingStartDate, maxLines);
            }

        }

        // To be tested as an scenario of the two main tests, and not separatedly,
        // therefore whithout annotation
        private void TestDayShift(DateTime loggingStartDate, int maxLines)
        {
            string loggingStartFileNamePrefix = loggingStartDate.ToString("yyyyMMdd");
            string loggingLastFileNamePrefix = logger._loggerCreationDate.ToString("yyyyMMdd");

            string searchPattern = loggingStartFileNamePrefix;
            DirectoryInfo di = new DirectoryInfo(localConf._logPath + "/");
            FileInfo[] filesStartDate = di.GetFiles(localConf._logPath + "/" + loggingStartFileNamePrefix);
            Assert.GreaterOrEqual(filesStartDate.Length, 1);

            FileInfo[] filesFinishDate = di.GetFiles(localConf._logPath + "/" + loggingLastFileNamePrefix);
            Assert.GreaterOrEqual(filesFinishDate.Length, 1);

            // If the logging process gets divided in two files during day shift
            var lineCount = File.ReadAllLines(logger._logFileName).Length;
            Assert.Less(15, lineCount);
        }

    }
}
